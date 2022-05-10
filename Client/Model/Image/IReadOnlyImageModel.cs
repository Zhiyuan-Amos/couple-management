namespace Couple.Client.Model.Image;

public interface IReadOnlyImageModel
{
    public Guid Id { get; }
    public DateTime TakenOn { get; }
    public int Order { get; }
    public DateOnly DoneDate { get; }
    public byte[] Data { get; }
    public bool IsFavourite { get; }
}