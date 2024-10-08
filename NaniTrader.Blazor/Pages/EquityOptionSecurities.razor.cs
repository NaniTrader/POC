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
using static NaniTrader.Services.Permissions.NaniTraderPermissions;
using System.Collections.Generic;

namespace NaniTrader.Pages
{
    public partial class EquityOptionSecurities
    {
        private IReadOnlyList<EquityOptionSecurityInListDto> EquityOptionSecuritiesList { get; set; } = new List<EquityOptionSecurityInListDto>();
        private List<SecurityParent> MasterSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> CreatingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> EditingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<UnderlyingSecurity> UnderlyingSecurityList { get; set; } = new List<UnderlyingSecurity>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateEquityOptionSecurity { get; set; }
        private bool CanEditEquityOptionSecurity { get; set; }
        private bool CanDeleteEquityOptionSecurity { get; set; }

        private CreateUpdateEquityOptionSecurityDto NewEquityOptionSecurity { get; set; }

        private Guid EditingEquityOptionSecurityId { get; set; }
        private int EditingSecurityParentGroupId { get; set; }
        private CreateUpdateEquityOptionSecurityDto EditingEquityOptionSecurity { get; set; }

        private Modal? CreateEquityOptionSecurityModal { get; set; }
        private Modal? EditEquityOptionSecurityModal { get; set; }

        private int CreatingSecurityParentGroupId { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public EquityOptionSecurities()
        {
            NewEquityOptionSecurity = new CreateUpdateEquityOptionSecurityDto();
            EditingEquityOptionSecurity = new CreateUpdateEquityOptionSecurityDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetEquityOptionSecuritiesAsync();
            await GetUnderlyingSecuritiesAsync();
            await GetSecuritiesParentAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateEquityOptionSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Create);

            CanEditEquityOptionSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Edit);

            CanDeleteEquityOptionSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Delete);
        }

        private async Task GetEquityOptionSecuritiesAsync()
        {
            var result = await SecurityAppService.GetEquityOptionSecurityPagedListWithNameFilterAsync(
                new EquityOptionSecurityListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            EquityOptionSecuritiesList = result.Items;
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

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<EquityOptionSecurityInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetEquityOptionSecuritiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateEquityOptionSecurityModal()
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewEquityOptionSecurity = new CreateUpdateEquityOptionSecurityDto();
            CreatingParentGroupChanged(CreatingSecurityParentGroupId);
            await (CreateEquityOptionSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateEquityOptionSecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.CreateEquityOptionSecurityAsync(NewEquityOptionSecurity);
                await GetEquityOptionSecuritiesAsync();
                await (CreateEquityOptionSecurityModal?.Hide() ?? Task.CompletedTask);
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

        private async void CloseCreateEquityOptionSecurityModal()
        {
            await (CreateEquityOptionSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditEquityOptionSecurityModal(EquityOptionSecurityInListDto equityOptionSecurityInList)
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();

            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingEquityOptionSecurityId = equityOptionSecurityInList.Id;
            EquityOptionSecurityDto equityOptionSecurity = await SecurityAppService.GetEquityOptionSecurityAsync(EditingEquityOptionSecurityId);
            EditingEquityOptionSecurity = ObjectMapper.Map<EquityOptionSecurityDto, CreateUpdateEquityOptionSecurityDto>(equityOptionSecurity);
            EditingSecurityParentGroupId = (int)MasterSecurityParentList.Where(x => x.Id == EditingEquityOptionSecurity.ParentId).First().ParentType;
            EditingParentGroupChanged(EditingSecurityParentGroupId);
            await (EditEquityOptionSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private void EditingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingEquityOptionSecurity.ParentId = default;
                    EditingEquityOptionSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingEquityOptionSecurity.ParentId = default;
                    EditingEquityOptionSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingEquityOptionSecurity.ParentId = default;
                    EditingEquityOptionSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                EditingEquityOptionSecurity.ParentId = default;
                EditingEquityOptionSecurity.UnderlyingId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async Task UpdateEquityOptionSecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.UpdateEquityOptionSecurityAsync(EditingEquityOptionSecurityId, EditingEquityOptionSecurity);
                await GetEquityOptionSecuritiesAsync();
                await (EditEquityOptionSecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditEquityOptionSecurityModal()
        {
            await (EditEquityOptionSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteEquityOptionSecurityAsync(EquityOptionSecurityInListDto equityOptionSecurityInList)
        {
            var confirmMessage = L["EquityOptionSecurityDeletionConfirmationMessage", equityOptionSecurityInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await SecurityAppService.DeleteEquityOptionSecurityAsync(equityOptionSecurityInList.Id);
            await GetEquityOptionSecuritiesAsync();
        }
    }
}

