﻿using Blazorise.DataGrid;
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
        private List<SecurityParent> MasterSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> CreatingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> EditingSecurityParentList { get; set; } = new List<SecurityParent>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateEquitySecurity { get; set; }
        private bool CanEditEquitySecurity { get; set; }
        private bool CanDeleteEquitySecurity { get; set; }

        private CreateUpdateEquitySecurityDto NewEquitySecurity { get; set; }

        private Guid EditingEquitySecurityId { get; set; }
        private int EditingSecurityParentGroupId { get; set; }
        private CreateUpdateEquitySecurityDto EditingEquitySecurity { get; set; }

        private Modal? CreateEquitySecurityModal { get; set; }
        private Modal? EditEquitySecurityModal { get; set; }

        private int CreatingSecurityParentGroupId { get; set; }

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
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewEquitySecurity = new CreateUpdateEquitySecurityDto();
            CreatingParentGroupChanged(CreatingSecurityParentGroupId);
            await (CreateEquitySecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateEquitySecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.CreateEquitySecurityAsync(NewEquitySecurity);
                await GetEquitySecuritiesAsync();
                await (CreateEquitySecurityModal?.Hide() ?? Task.CompletedTask);
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

        private async void CloseCreateEquitySecurityModal()
        {
            await (CreateEquitySecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditEquitySecurityModal(EquitySecurityInListDto equitySecurityInList)
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();

            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingEquitySecurityId = equitySecurityInList.Id;
            EquitySecurityDto equitySecurity = await SecurityAppService.GetEquitySecurityAsync(EditingEquitySecurityId);
            EditingEquitySecurity = ObjectMapper.Map<EquitySecurityDto, CreateUpdateEquitySecurityDto>(equitySecurity);
            EditingSecurityParentGroupId = (int)MasterSecurityParentList.Where(x => x.Id == EditingEquitySecurity.ParentId).First().ParentType;
            EditingParentGroupChanged(EditingSecurityParentGroupId);
            await (EditEquitySecurityModal?.Show() ?? Task.CompletedTask);
        }

        private void EditingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                    EditingEquitySecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                    EditingEquitySecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                    EditingEquitySecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                EditingEquitySecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async Task UpdateEquitySecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
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
