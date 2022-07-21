using Couple.Client.Shared;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Couple.Client.ProgramHelper;

public static class RootComponentMappingCollectionExtensions
{
    public static void AddApp(this RootComponentMappingCollection collection, 
        IWebAssemblyHostEnvironment environment,
        WebAssemblyHostConfiguration configuration)
    {
        if (environment.IsDevelopment() && !configuration.GetValue<bool>(Constants.IsAuthEnabled))
        {
            collection.Add<DevelopmentApp>("#app");
        }
        else
        {
            collection.Add<ProductionApp>("#app");
        }
    } 
}
