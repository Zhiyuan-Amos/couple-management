using Couple.Client.Features.Done.Models;
using Couple.Client.Features.Done.States;
using Couple.Client.Features.Image.Models;
using Couple.Client.Features.Synchronizer;
using Microsoft.AspNetCore.Components;

namespace Couple.Client.Features.Done.Components;

public partial class ReadOnlyListView : IDisposable
{
    private static bool s_isDataLoaded;
    [Inject] private DbContextProvider DbContextProvider { get; init; } = default!;
    [Inject] private NavigationManager NavigationManager { get; init; } = default!;
    [Inject] private DoneStateContainer DoneStateContainer { get; init; } = default!;

    private IEnumerable<KeyValuePair<DateOnly, ICollection<IDone>>> DateToItems =>
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
