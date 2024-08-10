using Blazorise.DataGrid;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using NaniTrader.Services.Permissions;
using NaniTrader.Services.Securities;
using Volo.Abp.ObjectMapping;
using NaniTrader.Entities;
using NaniTrader.Services.Brokers;
using NaniTrader.Services.Exchanges;
using NaniTrader.Services.MarketData;

namespace NaniTrader.Pages
{
    public partial class EquitySecurities
    {
        private IReadOnlyList<EquitySecurityInListDto> EquitySecuritiesList { get; set; } = new List<EquitySecurityInListDto>();
        private List<SecurityParent> SecurityParentList { get; set; } = new List<SecurityParent>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateEquitySecurity { get; set; }
        private bool CanEditEquitySecurity { get; set; }
        private bool CanDeleteEquitySecurity { get; set; }

        private CreateUpdateEquitySecurityDto NewEquitySecurity { get; set; }

        private Guid EditingEquitySecurityId { get; set; }
        private Guid EditingSecurityParentId { get; set; }
        private CreateUpdateEquitySecurityDto EditingEquitySecurity { get; set; }

        private Modal? CreateEquitySecurityModal { get; set; }
        private Modal? EditEquitySecurityModal { get; set; }

        private Guid CreatingSecurityParentId { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public EquitySecurities()
        {
            NewEquitySecurity = new CreateUpdateEquitySecurityDto();
            EditingEquitySecurity = new CreateUpdateEquitySecurityDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetEquitySecuritiesAsync();
            await GetSecuritiesParentAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateEquitySecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Create);

            CanEditEquitySecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Edit);

            CanDeleteEquitySecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Delete);
        }

        private async Task GetEquitySecuritiesAsync()
        {
            var result = await SecurityAppService.GetEquitySecurityPagedListWithNameFilterAsync(
                new EquitySecurityListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            EquitySecuritiesList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task GetSecuritiesParentAsync()
        {
            var brokers = await BrokerAppService.GetPagedListWithNameFilterAsync(
                new BrokerListFilterDto
                {
                    MaxResultCount = 100,
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );

            var exchanges = await ExchangeAppService.GetPagedListWithNameFilterAsync(
                new ExchangeListFilterDto
                {
                    MaxResultCount = 100,
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );

            var marketDataProviders = await MarketDataProviderAppService.GetPagedListWithNameFilterAsync(
                new MarketDataProviderListFilterDto
                {
                    MaxResultCount = 100,
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );

            SecurityParentList.AddRange(brokers.Items.Select(x => new SecurityParent { Id = x.Id, DisplayName = "Broker: " + x.Name }).ToList());
            SecurityParentList.AddRange(exchanges.Items.Select(x => new SecurityParent { Id = x.Id, DisplayName = "Exchange: " + x.Name }).ToList());
            SecurityParentList.AddRange(marketDataProviders.Items.Select(x => new SecurityParent { Id = x.Id, DisplayName = "MarketDataProvider: " + x.Name }).ToList());
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<EquitySecurityInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetEquitySecuritiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateEquitySecurityModal()
        {
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewEquitySecurity = new CreateUpdateEquitySecurityDto();
            await (CreateEquitySecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateEquitySecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                NewEquitySecurity.ParentId = CreatingSecurityParentId;
                await SecurityAppService.CreateEquitySecurityAsync(NewEquitySecurity);
                await GetEquitySecuritiesAsync();
                await (CreateEquitySecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseCreateEquitySecurityModal()
        {
            await (CreateEquitySecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditEquitySecurityModal(EquitySecurityInListDto equitySecurityInList)
        {
            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingEquitySecurityId = equitySecurityInList.Id;
            EquitySecurityDto equitySecurity = await SecurityAppService.GetEquitySecurityAsync(EditingEquitySecurityId);
            EditingEquitySecurity = ObjectMapper.Map<EquitySecurityDto, CreateUpdateEquitySecurityDto>(equitySecurity);
            EditingSecurityParentId = EditingEquitySecurity.ParentId;
            await (EditEquitySecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task UpdateEquitySecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                EditingEquitySecurity.ParentId = EditingSecurityParentId;
                await SecurityAppService.UpdateEquitySecurityAsync(EditingEquitySecurityId, EditingEquitySecurity);
                await GetEquitySecuritiesAsync();
                await (EditEquitySecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditEquitySecurityModal()
        {
            await (EditEquitySecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteEquitySecurityAsync(EquitySecurityInListDto equitySecurityInList)
        {
            var confirmMessage = L["EquitySecurityDeletionConfirmationMessage", equitySecurityInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await SecurityAppService.DeleteEquitySecurityAsync(equitySecurityInList.Id);
            await GetEquitySecuritiesAsync();
        }
    }
}
