using Blazorise.DataGrid;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using NaniTrader.Services.Permissions;
using NaniTrader.Services.Exchanges;

namespace NaniTrader.Pages
{
    public partial class Exchanges
    {
        private IReadOnlyList<ExchangeInListDto> ExchangeList { get; set; } = new List<ExchangeInListDto>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateExchange { get; set; }
        private bool CanEditExchange { get; set; }
        private bool CanDeleteExchange { get; set; }

        private CreateUpdateExchangeDto NewExchange { get; set; }

        private Guid EditingExchangeId { get; set; }
        private CreateUpdateExchangeDto EditingExchange { get; set; }

        private Modal? CreateExchangeModal { get; set; }
        private Modal? EditExchangeModal { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public Exchanges()
        {
            NewExchange = new CreateUpdateExchangeDto();
            EditingExchange = new CreateUpdateExchangeDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetExchangesAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateExchange = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Exchanges.Create);

            CanEditExchange = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Exchanges.Edit);

            CanDeleteExchange = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Exchanges.Delete);
        }

        private async Task GetExchangesAsync()
        {
            var result = await ExchangeAppService.GetPagedListWithNameFilterAsync(
                new ExchangeListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            ExchangeList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<ExchangeInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetExchangesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateExchangeModal()
        {
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewExchange = new CreateUpdateExchangeDto();
            await (CreateExchangeModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateExchangeAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await ExchangeAppService.CreateAsync(NewExchange);
                await GetExchangesAsync();
                await (CreateExchangeModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseCreateExchangeModal()
        {
            await (CreateExchangeModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditExchangeModal(ExchangeInListDto exchangeInList)
        {
            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingExchangeId = exchangeInList.Id;
            ExchangeDto Exchange = await ExchangeAppService.GetAsync(EditingExchangeId);
            EditingExchange = ObjectMapper.Map<ExchangeDto, CreateUpdateExchangeDto>(Exchange);
            await (EditExchangeModal?.Show() ?? Task.CompletedTask);
        }

        private async Task UpdateExchangeAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await ExchangeAppService.UpdateAsync(EditingExchangeId, EditingExchange);
                await GetExchangesAsync();
                await (EditExchangeModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditExchangeModal()
        {
            await (EditExchangeModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteExchangeAsync(ExchangeInListDto exchangeInList)
        {
            var confirmMessage = L["ExchangeDeletionConfirmationMessage", exchangeInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await ExchangeAppService.DeleteAsync(exchangeInList.Id);
            await GetExchangesAsync();
        }
    }
}
