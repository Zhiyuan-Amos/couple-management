using Couple.Client.Model.Done;
using Couple.Client.Model.Image;

namespace Couple.Client.States.Done;

public class DoneStateContainer : Notifier
{
    private readonly SortedDictionary<DateOnly, IReadOnlyList<IDone>> _dateToItems = new();
    private Dictionary<Guid, IReadOnlyImageModel> _idToImage = new();
    public IReadOnlyList<IReadOnlyImageModel> FavouriteImages { get; private set; } = new List<IReadOnlyImageModel>();

    public IReadOnlyDictionary<DateOnly, IReadOnlyList<IDone>> GetDateToItems() => _dateToItems;

    public void SetItems(IReadOnlyList<IDone> toSet)
    {
        _dateToItems.Clear();

        var toSetDict = toSet.GroupBy(d => d.DoneDate)
            .ToDictionary(g => g.Key, g => (IReadOnlyList<IDone>)g.OrderByDescending(d => d.Order).ToList());
        foreach (var (key, value) in toSetDict)
        {
            _dateToItems.Add(key, value);
        }

        _idToImage = toSet
            .OfType<IReadOnlyImageModel>()
            .ToDictionary(image => image.Id);

        FavouriteImages = toSet
            .OfType<IReadOnlyImageModel>()
            .Where(image => image.IsFavourite)
            .ToList();

        NotifyStateChanged();
    }

    public bool TryGetImage(Guid id, out IReadOnlyImageModel? readOnlyImage) =>
        _idToImage.TryGetValue(id, out readOnlyImage);
}
