using Couple.Client.Model.Done;

namespace Couple.Client.Model.Image;

public class ImageModel : IDone, IReadOnlyImageModel
{
    public ImageModel(DateTime takenOn, byte[] data, bool isFavourite)
    {
        TakenOn = takenOn;
        Data = data;
        IsFavourite = isFavourite;
    }

    public ImageModel(Guid id, DateTime takenOn, byte[] data, bool isFavourite) : this(takenOn, data, isFavourite) =>
        Id = id;

    // ReSharper disable once UnusedMember.Local
    private ImageModel(Guid id, DateTime takenOn, byte[] data, bool isFavourite, int order) : this(id, takenOn, data,
        isFavourite) => Order = order;

    private DateTime _takenOn { get; set; }

    public DateOnly TakenOnDate { get; private set; }

    public int Order { get; set; }

    public DateOnly DoneDate => TakenOnDate;

    public Guid Id { get; }

    public DateTime TakenOn
    {
        get => _takenOn;
        set
        {
            _takenOn = value;
            TakenOnDate = DateOnly.FromDateTime(_takenOn);
        }
    }

    public byte[] Data { get; set; }
    public bool IsFavourite { get; set; }
}
