using System;

namespace Couple.Client.Model.Image
{
    public class CreateImageModel
    {
        public Guid Id { get; }
        public DateTime TakenOn { get; }
        public byte[] Data { get; }
        public string TakenOnDate => TakenOn.ToString("dd/MM/yyyy");

        public CreateImageModel(Guid id, DateTime takenOn, byte[] data) => (Id, TakenOn, Data) = (id, takenOn, data);
    }
}
