using FluentValidation;

namespace ChronoLedger.Gateway.Setup;

public class GatewayInstaller
{
    public static void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddValidatorsFromAssemblyContaining<GatewayInstaller>();
    }
}