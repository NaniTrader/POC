using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace NaniTrader;

[Dependency(ReplaceServices = true)]
public class NaniTraderBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "NaniTrader";
}
