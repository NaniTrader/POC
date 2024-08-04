using Blazorise.DataGrid;
using Blazorise;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using NaniTrader.Services.Permissions;
using NaniTrader.Services.Brokers;

namespace NaniTrader.Pages
{
    public partial class Brokers
    {
        private IReadOnlyList<BrokerInListDto> BrokerList { get; set; } = new List<BrokerInListDto>();
        private int PageSize { get; } = LimitedResultRequestDto.DefaultMaxResultCount;
        private int CurrentPage { get; set; }
        private string CurrentSorting { get; set; } = string.Empty;
        private int TotalCount { get; set; }

        private bool CanCreateBroker { get; set; }
        private bool CanEditBroker { get; set; }
        private bool CanDeleteBroker { get; set; }

        private CreateUpdateBrokerDto NewBroker { get; set; }

        private Guid EditingBrokerId { get; set; }
        private CreateUpdateBrokerDto EditingBroker { get; set; }

        private Modal? CreateBrokerModal { get; set; }
        private Modal? EditBrokerModal { get; set; }

        private Validations? CreateValidationsRef;

        private Validations? EditValidationsRef;

        public Brokers()
        {
            NewBroker = new CreateUpdateBrokerDto();
            EditingBroker = new CreateUpdateBrokerDto();
        }

        protected override async Task OnInitializedAsync()
        {
            await SetPermissionsAsync();
            await GetBrokersAsync();
        }

        private async Task SetPermissionsAsync()
        {
            CanCreateBroker = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Brokers.Create);

            CanEditBroker = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Brokers.Edit);

            CanDeleteBroker = await AuthorizationService
                .IsGrantedAsync(NaniTraderPermissions.Brokers.Delete);
        }

        private async Task GetBrokersAsync()
        {
            var result = await BrokerAppService.GetPagedListWithNameFilterAsync(
                new BrokerListFilterDto
                {
                    MaxResultCount = PageSize,
                    SkipCount = CurrentPage * PageSize,
                    Sorting = CurrentSorting
                }
            );

            BrokerList = result.Items;
            TotalCount = (int)result.TotalCount;
        }

        private async Task OnDataGridReadAsync(DataGridReadDataEventArgs<BrokerInListDto> e)
        {
            CurrentSorting = e.Columns
                .Where(c => c.SortDirection != SortDirection.Default)
                .Select(c => c.Field + (c.SortDirection == SortDirection.Descending ? " DESC" : ""))
                .JoinAsString(",");
            CurrentPage = e.Page - 1;

            await GetBrokersAsync();

            await InvokeAsync(StateHasChanged);
        }

        private async void OpenCreateBrokerModal()
        {
            await (CreateValidationsRef?.ClearAll() ?? Task.CompletedTask);

            NewBroker = new CreateUpdateBrokerDto();
            await (CreateBrokerModal?.Show() ?? Task.CompletedTask);
        }

        private async void CloseCreateBrokerModal()
        {
            await (CreateBrokerModal?.Hide() ?? Task.CompletedTask);
        }

        private async void OpenEditBrokerModal(BrokerDto Broker)
        {
            await (EditValidationsRef?.ClearAll() ?? Task.CompletedTask);

            EditingBrokerId = Broker.Id;
            EditingBroker = ObjectMapper.Map<BrokerDto, CreateUpdateBrokerDto>(Broker);
            await (EditBrokerModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task DeleteBrokerAsync(BrokerDto Broker)
        {
            var confirmMessage = L["BrokerDeletionConfirmationMessage", Broker.Name];
            if (!await Message.Confirm(confirmMessage))
            {
                return;
            }

            await BrokerAppService.DeleteAsync(Broker.Id);
            await GetBrokersAsync();
        }

        private async void CloseEditBrokerModal()
        {
            await (EditBrokerModal?.Hide() ?? Task.CompletedTask);
        }

        private async Task CreateBrokerAsync()
        {
            var validationResult = await (CreateValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await BrokerAppService.CreateAsync(NewBroker);
                await GetBrokersAsync();
                await (CreateBrokerModal?.Hide() ?? Task.CompletedTask);
            }
        }

        private async Task UpdateBrokerAsync()
        {
            var validationResult = await (EditValidationsRef?.ValidateAll() ?? Task.FromResult(false));
            if (validationResult)
            {
                await BrokerAppService.UpdateAsync(EditingBrokerId, EditingBroker);
                await GetBrokersAsync();
                await (EditBrokerModal?.Hide() ?? Task.CompletedTask);
            }
        }
    }
}
