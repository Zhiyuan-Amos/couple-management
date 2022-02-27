using Couple.Client.Model.Image;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Done.Components;

public partial class ReadOnlyListView : IDisposable
{
    [Inject] private DbContextProvider DbContextProvider { get; init; }
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }

    public void Dispose() => DoneStateContainer.OnChange -= StateHasChanged;

    protected override async Task OnInitializedAsync()
    {
        DoneStateContainer.OnChange += StateHasChanged;
        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        var done = await db.GetIDone();
        DoneStateContainer.SetItems(done);
    }

    private void EditImage(ImageModel selectedImage) => NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
}
