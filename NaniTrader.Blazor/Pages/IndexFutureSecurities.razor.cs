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
    public partial class IndexFutureSecurities
    {
        private IReadOnlyList<IndexFutureSecurityInListDto> IndexFutureSecuritiesList { get; set; } = new List<IndexFutureSecurityInListDto>();
        private List<SecurityParent> MasterSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> CreatingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<SecurityParent> EditingSecurityParentList { get; set; } = new List<SecurityParent>();
        private List<UnderlyingSecurity> UnderlyingSecurityList { get; set; } = new List<UnderlyingSecurity>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateIndexFutureSecurity { get; set; }
        private bool CanEditIndexFutureSecurity { get; set; }
        private bool CanDeleteIndexFutureSecurity { get; set; }

        private CreateUpdateIndexFutureSecurityDto NewIndexFutureSecurity { get; set; }

        private Guid EditingIndexFutureSecurityId { get; set; }
        private int EditingSecurityParentGroupId { get; set; }
        private CreateUpdateIndexFutureSecurityDto EditingIndexFutureSecurity { get; set; }

        private Modal? CreateIndexFutureSecurityModal { get; set; }
        private Modal? EditIndexFutureSecurityModal { get; set; }

        private int CreatingSecurityParentGroupId { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public IndexFutureSecurities()
        {
            NewIndexFutureSecurity = new CreateUpdateIndexFutureSecurityDto();
            EditingIndexFutureSecurity = new CreateUpdateIndexFutureSecurityDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetIndexFutureSecuritiesAsync();
            await GetUnderlyingSecuritiesAsync();
            await GetSecuritiesParentAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateIndexFutureSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Create);

            CanEditIndexFutureSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Edit);

            CanDeleteIndexFutureSecurity = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Securities.Delete);
        }

        private async Task GetIndexFutureSecuritiesAsync()
        {
            var result = await SecurityAppService.GetIndexFutureSecurityPagedListWithNameFilterAsync(
                new IndexFutureSecurityListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            IndexFutureSecuritiesList = result.Items;
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

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<IndexFutureSecurityInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetIndexFutureSecuritiesAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateIndexFutureSecurityModal()
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewIndexFutureSecurity = new CreateUpdateIndexFutureSecurityDto();
            CreatingParentGroupChanged(CreatingSecurityParentGroupId);
            await (CreateIndexFutureSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private async Task CreateIndexFutureSecurityAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.CreateIndexFutureSecurityAsync(NewIndexFutureSecurity);
                await GetIndexFutureSecuritiesAsync();
                await (CreateIndexFutureSecurityModal?.Hide() ?? Task.CompletedTask);
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

        private async void CloseCreateIndexFutureSecurityModal()
        {
            await (CreateIndexFutureSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditIndexFutureSecurityModal(IndexFutureSecurityInListDto indexFutureSecurityInList)
        {
            CreatingSecurityParentList = MasterSecurityParentList.ToList();
            EditingSecurityParentList = MasterSecurityParentList.ToList();

            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingIndexFutureSecurityId = indexFutureSecurityInList.Id;
            IndexFutureSecurityDto indexFutureSecurity = await SecurityAppService.GetIndexFutureSecurityAsync(EditingIndexFutureSecurityId);
            EditingIndexFutureSecurity = ObjectMapper.Map<IndexFutureSecurityDto, CreateUpdateIndexFutureSecurityDto>(indexFutureSecurity);
            EditingSecurityParentGroupId = (int)MasterSecurityParentList.Where(x => x.Id == EditingIndexFutureSecurity.ParentId).First().ParentType;
            EditingParentGroupChanged(EditingSecurityParentGroupId);
            await (EditIndexFutureSecurityModal?.Show() ?? Task.CompletedTask);
        }

        private void EditingParentGroupChanged(int newParentGroupId)
        {
            if (newParentGroupId == 1)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingIndexFutureSecurity.ParentId = default;
                    EditingIndexFutureSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Exchange).ToList();
            }
            else if (newParentGroupId == 2)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingIndexFutureSecurity.ParentId = default;
                    EditingIndexFutureSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.Broker).ToList();
            }
            else if (newParentGroupId == 3)
            {
                if (EditingSecurityParentGroupId != newParentGroupId)
                {
                    EditingIndexFutureSecurity.ParentId = default;
                    EditingIndexFutureSecurity.UnderlyingId = default;
                }

                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = MasterSecurityParentList.Where(x => x.ParentType == ParentType.MarketDataProvider).ToList();
            }
            else
            {
                EditingIndexFutureSecurity.ParentId = default;
                EditingIndexFutureSecurity.UnderlyingId = default;
                EditingSecurityParentGroupId = newParentGroupId;
                EditingSecurityParentList = new List<SecurityParent>();
            }
        }

        private async Task UpdateIndexFutureSecurityAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await SecurityAppService.UpdateIndexFutureSecurityAsync(EditingIndexFutureSecurityId, EditingIndexFutureSecurity);
                await GetIndexFutureSecuritiesAsync();
                await (EditIndexFutureSecurityModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async void CloseEditIndexFutureSecurityModal()
        {
            await (EditIndexFutureSecurityModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteIndexFutureSecurityAsync(IndexFutureSecurityInListDto indexFutureSecurityInList)
        {
            var confirmMessage = L["IndexFutureSecurityDeletionConfirmationMessage", indexFutureSecurityInList.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await SecurityAppService.DeleteIndexFutureSecurityAsync(indexFutureSecurityInList.Id);
            await GetIndexFutureSecuritiesAsync();
        }
    }
}

