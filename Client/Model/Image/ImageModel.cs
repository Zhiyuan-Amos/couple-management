using System;

namespace Couple.Client.Model.Image
{
    public class ImageModel
    {
        public Guid Id { get; }
        public DateTime TakenOn { get; }
        public byte[] Data { get; }

        public ImageModel(Guid id, DateTime takenOn, byte[] data) => (Id, TakenOn, Data) = (id, takenOn, data);
    }
}
