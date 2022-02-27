using Couple.Client.Model.Done;
using Couple.Client.Model.Image;

namespace Couple.Client.States.Done;

public class DoneStateContainer : Notifier
{
    private readonly SortedDictionary<DateOnly, IReadOnlyList<IDone>> _dateToItems = new();
    private Dictionary<Guid, ImageModel> _idToImage = new();

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
            .OfType<ImageModel>()
            .ToDictionary(image => image.Id);
        NotifyStateChanged();
    }

    public bool TryGetImage(Guid id, out ImageModel image)
    {
        if (!_idToImage.TryGetValue(id, out image))
        {
            return false;
        }

        return true;
    }
}
