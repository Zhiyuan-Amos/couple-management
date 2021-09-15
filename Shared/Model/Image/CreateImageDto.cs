using System;

namespace Couple.Shared.Model.Image
{
    public class CreateImageDto
    {
        public Guid Id { get; set; }
        public DateTime TakenOn { get; set; }
        public byte[] Data { get; set; }
    }
}
