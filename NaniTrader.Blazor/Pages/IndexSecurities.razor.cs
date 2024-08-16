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
    public partial class IndexSecurities
    {
        private IReadOnlyList<IndexSecurityInListDto> IndexSecuritiesList { get; set; } = new List<IndexSecurityInListDto>();
        private List<SecurityParent> MasterSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> CreatingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> EditingSecurityParentList { get; set; } = new List<SecurityParent>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateIndexSecurity { get; set; }
        private bool CanEditIndexSecurity { get; set; }
        private bool CanDeleteIndexSecurity { get; set; }

        private CreateUpdateIndexSecurityDto NewIndexSecurity { get; set; }

        private Guid EditingIndexSecurityId { get; set; }
        private int EditingSecurityParentGroupId { get; set; }
        private CreateUpdateIndexSecurityDto EditingIndexSecurity { get; set; }

        private Modal? CreateIndexSecurityModal { get; set; }
        private Modal? EditIndexSecurityModal { get; set; }

        private int CreatingSecurityParentGroupId { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public IndexSecurities()
        {
            NewIndexSecurity = new CreateUpdateIndexSecurityDto();
            EditingIndexSecurity = new CreateUpdateIndexSecurityDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetIndexSecuritiesAsync();
            await GetSecuritiesParentAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateIndexSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Create);

            CanEditIndexSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Edit);

            CanDeleteIndexSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Delete);
        }

        private async Task GetIndexSecuritiesAsync()
        {
            var result = await SecurityAppService.GetIndexSecurityPagedListWithNameFilterAsync(
                new IndexSecurityListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            IndexSecuritiesList = result.Items;
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

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IndexSecurityInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetIndexSecuritiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateIndexSecurityModal()
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewIndexSecurity = new CreateUpdateIndexSecurityDto();
            CreatingParentGroupChanged(CreatingSecurityParentGroupId);
            await (CreateIndexSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateIndexSecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.CreateIndexSecurityAsync(NewIndexSecurity);
                await GetIndexSecuritiesAsync();
                await (CreateIndexSecurityModal?.Hide() ?? Task.CompletedTask);
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

        private async void CloseCreateIndexSecurityModal()
        {
            await (CreateIndexSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditIndexSecurityModal(IndexSecurityInListDto indexSecurityInList)
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();

            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingIndexSecurityId = indexSecurityInList.Id;
            IndexSecurityDto indexSecurity = await SecurityAppService.GetIndexSecurityAsync(EditingIndexSecurityId);
            EditingIndexSecurity = ObjectMapper.Map<IndexSecurityDto, CreateUpdateIndexSecurityDto>(indexSecurity);
            EditingSecurityParentGroupId = (int)MasterSecurityParentList.Where(x => x.Id == EditingIndexSecurity.ParentId).First().ParentType;
            EditingParentGroupChanged(EditingSecurityParentGroupId);
            await (EditIndexSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private void EditingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                    EditingIndexSecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                    EditingIndexSecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                    EditingIndexSecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                EditingIndexSecurity.ParentId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async Task UpdateIndexSecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.UpdateIndexSecurityAsync(EditingIndexSecurityId, EditingIndexSecurity);
                await GetIndexSecuritiesAsync();
                await (EditIndexSecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditIndexSecurityModal()
        {
            await (EditIndexSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteIndexSecurityAsync(IndexSecurityInListDto indexSecurityInList)
        {
            var confirmMessage = L["IndexSecurityDeletionConfirmationMessage", indexSecurityInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await SecurityAppService.DeleteIndexSecurityAsync(indexSecurityInList.Id);
            await GetIndexSecuritiesAsync();
        }
    }
}
