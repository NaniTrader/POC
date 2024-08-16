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
    public partial class IndexOptionSecurities
    {
        private IReadOnlyList<IndexOptionSecurityInListDto> IndexOptionSecuritiesList { get; set; } = new List<IndexOptionSecurityInListDto>();
        private List<SecurityParent> MasterSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> CreatingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> EditingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<UnderlyingSecurity> UnderlyingSecurityList { get; set; } = new List<UnderlyingSecurity>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateIndexOptionSecurity { get; set; }
        private bool CanEditIndexOptionSecurity { get; set; }
        private bool CanDeleteIndexOptionSecurity { get; set; }

        private CreateUpdateIndexOptionSecurityDto NewIndexOptionSecurity { get; set; }

        private Guid EditingIndexOptionSecurityId { get; set; }
        private int EditingSecurityParentGroupId { get; set; }
        private CreateUpdateIndexOptionSecurityDto EditingIndexOptionSecurity { get; set; }

        private Modal? CreateIndexOptionSecurityModal { get; set; }
        private Modal? EditIndexOptionSecurityModal { get; set; }

        private int CreatingSecurityParentGroupId { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public IndexOptionSecurities()
        {
            NewIndexOptionSecurity = new CreateUpdateIndexOptionSecurityDto();
            EditingIndexOptionSecurity = new CreateUpdateIndexOptionSecurityDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetIndexOptionSecuritiesAsync();
            await GetUnderlyingSecuritiesAsync();
            await GetSecuritiesParentAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateIndexOptionSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Create);

            CanEditIndexOptionSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Edit);

            CanDeleteIndexOptionSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Delete);
        }

        private async Task GetIndexOptionSecuritiesAsync()
        {
            var result = await SecurityAppService.GetIndexOptionSecurityPagedListWithNameFilterAsync(
                new IndexOptionSecurityListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            IndexOptionSecuritiesList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task GetUnderlyingSecuritiesAsync()
        {
            // TODO Filter Underlying securities with parent type and call the list upon parent type change in new and edit modals
            var underlyingSecurities = await SecurityAppService.GetIndexSecurityPagedListWithNameFilterAsync(
                new IndexSecurityListFilterDto
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

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IndexOptionSecurityInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetIndexOptionSecuritiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateIndexOptionSecurityModal()
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewIndexOptionSecurity = new CreateUpdateIndexOptionSecurityDto();
            CreatingParentGroupChanged(CreatingSecurityParentGroupId);
            await (CreateIndexOptionSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateIndexOptionSecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.CreateIndexOptionSecurityAsync(NewIndexOptionSecurity);
                await GetIndexOptionSecuritiesAsync();
                await (CreateIndexOptionSecurityModal?.Hide() ?? Task.CompletedTask);
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

        private async void CloseCreateIndexOptionSecurityModal()
        {
            await (CreateIndexOptionSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditIndexOptionSecurityModal(IndexOptionSecurityInListDto indexOptionSecurityInList)
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();

            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingIndexOptionSecurityId = indexOptionSecurityInList.Id;
            IndexOptionSecurityDto indexOptionSecurity = await SecurityAppService.GetIndexOptionSecurityAsync(EditingIndexOptionSecurityId);
            EditingIndexOptionSecurity = ObjectMapper.Map<IndexOptionSecurityDto, CreateUpdateIndexOptionSecurityDto>(indexOptionSecurity);
            EditingSecurityParentGroupId = (int)MasterSecurityParentList.Where(x => x.Id == EditingIndexOptionSecurity.ParentId).First().ParentType;
            EditingParentGroupChanged(EditingSecurityParentGroupId);
            await (EditIndexOptionSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private void EditingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingIndexOptionSecurity.ParentId = default;
                    EditingIndexOptionSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingIndexOptionSecurity.ParentId = default;
                    EditingIndexOptionSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingIndexOptionSecurity.ParentId = default;
                    EditingIndexOptionSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                EditingIndexOptionSecurity.ParentId = default;
                EditingIndexOptionSecurity.UnderlyingId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async Task UpdateIndexOptionSecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.UpdateIndexOptionSecurityAsync(EditingIndexOptionSecurityId, EditingIndexOptionSecurity);
                await GetIndexOptionSecuritiesAsync();
                await (EditIndexOptionSecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditIndexOptionSecurityModal()
        {
            await (EditIndexOptionSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteIndexOptionSecurityAsync(IndexOptionSecurityInListDto indexOptionSecurityInList)
        {
            var confirmMessage = L["IndexOptionSecurityDeletionConfirmationMessage", indexOptionSecurityInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await SecurityAppService.DeleteIndexOptionSecurityAsync(indexOptionSecurityInList.Id);
            await GetIndexOptionSecuritiesAsync();
        }
    }
}

