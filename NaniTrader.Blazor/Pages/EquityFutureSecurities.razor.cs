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
using static NaniTrader.Services.Permissions.NaniTraderPermissions;
using System.Collections.Generic;

namespace NaniTrader.Pages
{
    public partial class EquityFutureSecurities
    {
        private IReadOnlyList<EquityFutureSecurityInListDto> EquityFutureSecuritiesList { get; set; } = new List<EquityFutureSecurityInListDto>();
        private List<SecurityParent> MasterSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> CreatingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> EditingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<UnderlyingSecurity> UnderlyingSecurityList { get; set; } = new List<UnderlyingSecurity>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateEquityFutureSecurity { get; set; }
        private bool CanEditEquityFutureSecurity { get; set; }
        private bool CanDeleteEquityFutureSecurity { get; set; }

        private CreateUpdateEquityFutureSecurityDto NewEquityFutureSecurity { get; set; }

        private Guid EditingEquityFutureSecurityId { get; set; }
        private int EditingSecurityParentGroupId { get; set; }
        private CreateUpdateEquityFutureSecurityDto EditingEquityFutureSecurity { get; set; }

        private Modal? CreateEquityFutureSecurityModal { get; set; }
        private Modal? EditEquityFutureSecurityModal { get; set; }

        private int CreatingSecurityParentGroupId { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public EquityFutureSecurities()
        {
            NewEquityFutureSecurity = new CreateUpdateEquityFutureSecurityDto();
            EditingEquityFutureSecurity = new CreateUpdateEquityFutureSecurityDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetEquityFutureSecuritiesAsync();
            await GetUnderlyingSecuritiesAsync();
            await GetSecuritiesParentAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateEquityFutureSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Create);

            CanEditEquityFutureSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Edit);

            CanDeleteEquityFutureSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Delete);
        }

        private async Task GetEquityFutureSecuritiesAsync()
        {
            var result = await SecurityAppService.GetEquityFutureSecurityPagedListWithNameFilterAsync(
                new EquityFutureSecurityListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            EquityFutureSecuritiesList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task GetUnderlyingSecuritiesAsync()
        {
            // TODO Filter Underlying securities with parent type and call the list upon parent type change in new and edit modals
            var underlyingSecurities = await SecurityAppService.GetEquitySecurityPagedListWithNameFilterAsync(
                new EquitySecurityListFilterDto
                {
                    MaxResultCount = 1000, // TODO implement Get All
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );
            UnderlyingSecurityList.AddRange(underlyingSecurities.Items.Select(x => new UnderlyingSecurity { Id = x.Id, DisplayName = x.Name }).ToList());
        }

        private async Task GetSecuritiesParentAsync()
        {
            var brokers = await BrokerAppService.GetPagedListWithNameFilterAsync(
                new BrokerListFilterDto
                {
                    MaxResultCount = 100, // TODO implement Get All
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );

            var exchanges = await ExchangeAppService.GetPagedListWithNameFilterAsync(
                new ExchangeListFilterDto
                {
                    MaxResultCount = 100, // TODO implement Get All
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );

            var marketDataProviders = await MarketDataProviderAppService.GetPagedListWithNameFilterAsync(
                new MarketDataProviderListFilterDto
                {
                    MaxResultCount = 100, // TODO implement Get All
                    SkipCount = 0,
                    Sorting = string.Empty
                }
            );

            MasterSecurityParentList.AddRange(brokers.Items.Select(x => new SecurityParent { Id = x.Id, DisplayName = x.Name, ParentType = ParentType.Broker }).ToList());
            MasterSecurityParentList.AddRange(exchanges.Items.Select(x => new SecurityParent { Id = x.Id, DisplayName = x.Name, ParentType = ParentType.Exchange }).ToList());
            MasterSecurityParentList.AddRange(marketDataProviders.Items.Select(x => new SecurityParent { Id = x.Id, DisplayName = x.Name, ParentType = ParentType.MarketDataProvider }).ToList());
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            CreatingSecurityParentGroupId = 0;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<EquityFutureSecurityInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetEquityFutureSecuritiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateEquityFutureSecurityModal()
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewEquityFutureSecurity = new CreateUpdateEquityFutureSecurityDto();
            CreatingParentGroupChanged(CreatingSecurityParentGroupId);
            await (CreateEquityFutureSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateEquityFutureSecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.CreateEquityFutureSecurityAsync(NewEquityFutureSecurity);
                await GetEquityFutureSecuritiesAsync();
                await (CreateEquityFutureSecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private void CreatingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                CreatingSecurityParentGroupId = newParentGroupId;
                CreatingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                CreatingSecurityParentGroupId = newParentGroupId;
                CreatingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                CreatingSecurityParentGroupId = newParentGroupId;
                CreatingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                CreatingSecurityParentGroupId = newParentGroupId;
                CreatingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async void CloseCreateEquityFutureSecurityModal()
        {
            await (CreateEquityFutureSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditEquityFutureSecurityModal(EquityFutureSecurityInListDto equityFutureSecurityInList)
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();

            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingEquityFutureSecurityId = equityFutureSecurityInList.Id;
            EquityFutureSecurityDto equityFutureSecurity = await SecurityAppService.GetEquityFutureSecurityAsync(EditingEquityFutureSecurityId);
            EditingEquityFutureSecurity = ObjectMapper.Map<EquityFutureSecurityDto, CreateUpdateEquityFutureSecurityDto>(equityFutureSecurity);
            EditingSecurityParentGroupId = (int)MasterSecurityParentList.Where(x => x.Id == EditingEquityFutureSecurity.ParentId).First().ParentType;
            EditingParentGroupChanged(EditingSecurityParentGroupId);
            await (EditEquityFutureSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private void EditingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingEquityFutureSecurity.ParentId = default;
                    EditingEquityFutureSecurity.UnderlyingId = default;
                }
                    
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingEquityFutureSecurity.ParentId = default;
                    EditingEquityFutureSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingEquityFutureSecurity.ParentId = default;
                    EditingEquityFutureSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                EditingEquityFutureSecurity.ParentId = default;
                EditingEquityFutureSecurity.UnderlyingId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async Task UpdateEquityFutureSecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.UpdateEquityFutureSecurityAsync(EditingEquityFutureSecurityId, EditingEquityFutureSecurity);
                await GetEquityFutureSecuritiesAsync();
                await (EditEquityFutureSecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditEquityFutureSecurityModal()
        {
            await (EditEquityFutureSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteEquityFutureSecurityAsync(EquityFutureSecurityInListDto equityFutureSecurityInList)
        {
            var confirmMessage = L["EquityFutureSecurityDeletionConfirmationMessage", equityFutureSecurityInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await SecurityAppService.DeleteEquityFutureSecurityAsync(equityFutureSecurityInList.Id);
            await GetEquityFutureSecuritiesAsync();
        }
    }
}
