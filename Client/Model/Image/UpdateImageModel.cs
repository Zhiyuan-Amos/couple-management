using System;

namespace Couple.Client.Model.Image
{
    public class UpdateImageModel
    {
        public Guid Id { get; }
        public DateTime TakenOn { get; }
        public byte[] Data { get; }
        public bool IsFavourite { get; set; }
        public string TakenOnDate => TakenOn.ToString("dd/MM/yyyy");

        public UpdateImageModel(Guid id, DateTime takenOn, byte[] data, bool isFavourite) =>
            (Id, Data, TakenOn, IsFavourite) = (id, data, takenOn, isFavourite);
    }
}
