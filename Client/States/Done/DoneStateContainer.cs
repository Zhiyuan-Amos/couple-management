using Couple.Client.Model.Image;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Couple.Client.States.Done
{
    public class DoneStateContainer : Notifier
    {
        private readonly SortedDictionary<DateOnly, IReadOnlyList<object>> _dateToItems = new();
        private Dictionary<Guid, ImageModel> _idToImage = new();

        public IReadOnlyDictionary<DateOnly, IReadOnlyList<object>> GetDateToItems() => _dateToItems;

        public void SetDateToItems(IDictionary<DateOnly, IReadOnlyList<object>> toSet)
        {
            _dateToItems.Clear();
            foreach (var (key, value) in toSet)
            {
                _dateToItems.Add(key, value);
            }

            _idToImage = toSet.Values
                .SelectMany(items => items)
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
}
