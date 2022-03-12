using Couple.Client.Model.Done;
using Couple.Client.Model.Image;
using Couple.Client.Services.Synchronizer;
using Couple.Client.States.Done;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Pages.Done.Components;

public partial class ReadOnlyListView : IDisposable
{
    private static bool s_isDataLoaded;
    [Inject] private DbContextProvider DbContextProvider { get; init; }
    [Inject] private NavigationManager NavigationManager { get; init; }
    [Inject] private DoneStateContainer DoneStateContainer { get; init; }

    private IEnumerable<KeyValuePair<DateOnly, IReadOnlyList<IDone>>> DateToItems =>
        DoneStateContainer
            .GetDateToItems()
            .Reverse();

    public void Dispose() => DoneStateContainer.OnChange -= StateHasChanged;

    protected override async Task OnInitializedAsync()
    {
        DoneStateContainer.OnChange += StateHasChanged;

        if (s_isDataLoaded)
        {
            return;
        }

        await using var db = await DbContextProvider.GetPreparedDbContextAsync();
        var done = await db.GetIDone();
        DoneStateContainer.SetItems(done);

        s_isDataLoaded = true;
    }

    private void EditImage(IReadOnlyImageModel selectedImage) =>
        NavigationManager.NavigateTo($"/image/{selectedImage.Id}");
}
