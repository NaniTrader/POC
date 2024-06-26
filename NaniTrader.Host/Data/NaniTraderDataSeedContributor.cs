using JetBrains.Annotations;
using Microsoft.Extensions.Localization;
using NaniTrader.Localization;
using OpenIddict.Abstractions;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace NaniTrader.Data;

/* Creates initial data that is needed to property run the application
 * and make client-to-server communication possible.
 */
public class NaniTraderDataSeedContributor : IDataSeedContributor, ITransientDependency
{
    private readonly IConfiguration _configuration;
    private readonly IStringLocalizer<NaniTraderResource> L;

    public NaniTraderDataSeedContributor(
        IConfiguration configuration,
        IStringLocalizer<NaniTraderResource> l)
    {
        _configuration = configuration;
        L = l;
    }

    [UnitOfWork]
    public virtual async Task SeedAsync(DataSeedContext context)
    {
        await Task.CompletedTask;
    }
}
