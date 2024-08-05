using Blazorise.DataGrid;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using NaniTrader.Services.Permissions;
using NaniTrader.Services.MarketData;

namespace NaniTrader.Pages
{
    public partial class MarketDataProviders
    {
        private IReadOnlyList<MarketDataProviderInListDto> MarketDataProviderList { get; set; } = new List<MarketDataProviderInListDto>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateMarketDataProvider { get; set; }
        private bool CanEditMarketDataProvider { get; set; }
        private bool CanDeleteMarketDataProvider { get; set; }

        private CreateUpdateMarketDataProviderDto NewMarketDataProvider { get; set; }

        private Guid EditingMarketDataProviderId { get; set; }
        private CreateUpdateMarketDataProviderDto EditingMarketDataProvider { get; set; }

        private Modal? CreateMarketDataProviderModal { get; set; }
        private Modal? EditMarketDataProviderModal { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public MarketDataProviders()
        {
            NewMarketDataProvider = new CreateUpdateMarketDataProviderDto();
            EditingMarketDataProvider = new CreateUpdateMarketDataProviderDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetMarketDataProvidersAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateMarketDataProvider = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.MarketDataProviders.Create);

            CanEditMarketDataProvider = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.MarketDataProviders.Edit);

            CanDeleteMarketDataProvider = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.MarketDataProviders.Delete);
        }

        private async Task GetMarketDataProvidersAsync()
        {
            var result = await MarketDataProviderAppService.GetPagedListWithNameFilterAsync(
                new MarketDataProviderListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            MarketDataProviderList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<MarketDataProviderInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetMarketDataProvidersAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateMarketDataProviderModal()
        {
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewMarketDataProvider = new CreateUpdateMarketDataProviderDto();
            await (CreateMarketDataProviderModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateMarketDataProviderAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await MarketDataProviderAppService.CreateAsync(NewMarketDataProvider);
                await GetMarketDataProvidersAsync();
                await (CreateMarketDataProviderModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseCreateMarketDataProviderModal()
        {
            await (CreateMarketDataProviderModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditMarketDataProviderModal(MarketDataProviderInListDto exchangeInList)
        {
            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingMarketDataProviderId = exchangeInList.Id;
            MarketDataProviderDto MarketDataProvider = await MarketDataProviderAppService.GetAsync(EditingMarketDataProviderId);
            EditingMarketDataProvider = ObjectMapper.Map<MarketDataProviderDto, CreateUpdateMarketDataProviderDto>(MarketDataProvider);
            await (EditMarketDataProviderModal?.Show() ?? Task.CompletedTask);
        }

        private async Task UpdateMarketDataProviderAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await MarketDataProviderAppService.UpdateAsync(EditingMarketDataProviderId, EditingMarketDataProvider);
                await GetMarketDataProvidersAsync();
                await (EditMarketDataProviderModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditMarketDataProviderModal()
        {
            await (EditMarketDataProviderModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteMarketDataProviderAsync(MarketDataProviderInListDto exchangeInList)
        {
            var confirmMessage = L["MarketDataProviderDeletionConfirmationMessage", exchangeInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await MarketDataProviderAppService.DeleteAsync(exchangeInList.Id);
            await GetMarketDataProvidersAsync();
        }
    }
}
