using Couple.Client.Features.Done.Models;
using Couple.Client.Features.Image.Models;
using Couple.Client.Shared.States;

namespace Couple.Client.Features.Done.States;

public class DoneStateContainer : Notifier
{
    private readonly SortedDictionary<DateOnly, ICollection<IDone>> _dateToItems = new();
    private List<IReadOnlyImageModel> _favouriteImages = new();
    private Dictionary<Guid, IReadOnlyImageModel> _idToImage = new();

    public IReadOnlyList<IReadOnlyImageModel> FavouriteImages => _favouriteImages;
    public IReadOnlyDictionary<DateOnly, ICollection<IDone>> GetDateToItems() => _dateToItems;

    public void SetItems(IReadOnlyList<IDone> toSet)
    {
        _dateToItems.Clear();

        var toSetDict = toSet.GroupBy(d => d.DoneDate)
            .ToDictionary(g => g.Key, g => (ICollection<IDone>)g.OrderByDescending(d => d.Order).ToList());
        foreach (var (key, value) in toSetDict)
        {
            _dateToItems.Add(key, value);
        }

        _idToImage = toSet
            .OfType<IReadOnlyImageModel>()
            .ToDictionary(image => image.Id);

        _favouriteImages = toSet
            .OfType<IReadOnlyImageModel>()
            .Where(image => image.IsFavourite)
            .ToList();

        NotifyStateChanged();
    }

    public void AddImage(ImageModel image)
    {
        var hasDoneOnDate = _dateToItems.TryGetValue(image.DoneDate, out var existingDone);
        if (hasDoneOnDate)
        {
            var list = (existingDone as List<IDone>)!;
            list.Add(image);
        }
        else
        {
            _dateToItems.Add(image.DoneDate, new List<IDone> { image });
        }

        _idToImage.Add(image.Id, image);

        if (image.IsFavourite)
        {
            _favouriteImages.Add(image);
        }

        NotifyStateChanged();
    }

    public void UpdateImage(ImageModel image)
    {
        _idToImage.Remove(image.Id, out var oldImage);

        var existingOldDone = (List<IDone>)_dateToItems[oldImage!.DoneDate];
        existingOldDone.Remove((ImageModel)oldImage);

        if (oldImage.IsFavourite)
        {
            _favouriteImages.Remove(oldImage);
        }

        var hasDoneOnDate = _dateToItems.TryGetValue(image.DoneDate, out var existingNewDone);
        if (hasDoneOnDate)
        {
            var list = (existingNewDone as List<IDone>)!;
            list.Add(image);
        }
        else
        {
            _dateToItems.Add(image.DoneDate, new List<IDone> { image });
        }

        _idToImage.Add(image.Id, image);

        if (image.IsFavourite)
        {
            _favouriteImages.Add(image);
        }

        NotifyStateChanged();
    }

    public void DeleteImage(Guid id)
    {
        _idToImage.Remove(id, out var oldImage);

        var existingDone = (List<IDone>)_dateToItems[oldImage!.DoneDate];
        if (existingDone.Count == 1)
        {
            _dateToItems.Remove(oldImage.DoneDate);
        }
        else
        {
            existingDone.Remove((ImageModel)oldImage);
        }

        if (oldImage.IsFavourite)
        {
            _favouriteImages.Remove(oldImage);
        }

        NotifyStateChanged();
    }

    public void UpdateIssue(DoneIssueModel issue)
    {
        var hasDoneOnDate = _dateToItems.TryGetValue(issue.DoneDate, out var existingDone);
        if (hasDoneOnDate)
        {
            var list = (List<IDone>)existingDone!;
            var toRemove = list.SingleOrDefault(d => d.Id == issue.Id);

            if (toRemove is not null)
            {
                list.Remove(toRemove);
            }

            list.Add(issue);
        }
        else
        {
            _dateToItems.Add(issue.DoneDate, new List<IDone> { issue });
        }

        NotifyStateChanged();
    }

    public bool TryGetImage(Guid id, out IReadOnlyImageModel? readOnlyImage) =>
        _idToImage.TryGetValue(id, out readOnlyImage);
}
