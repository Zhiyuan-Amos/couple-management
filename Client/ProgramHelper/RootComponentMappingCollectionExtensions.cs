using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Couple.Client.ProgramHelper;

public static class RootComponentMappingCollectionExtensions
{
    public static void AddApp(this RootComponentMappingCollection collection, IWebAssemblyHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            collection.Add<DevelopmentApp>("#app");
        }
        else
        {
            collection.Add<ProductionApp>("#app");
        }
    } 
}
