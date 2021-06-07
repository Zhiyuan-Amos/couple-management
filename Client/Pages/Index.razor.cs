using Couple.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Couple.Client.Pages
{
    public partial class Index
    {
        [Inject] private IServiceProvider ServiceProvider { get; init; }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                ServiceProvider.GetRequiredService<Synchronizer>();
            }
        }
    }
}
