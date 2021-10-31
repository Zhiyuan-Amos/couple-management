using System;

namespace Couple.Api.Model
{
    public class Image
    {
        public Guid Id { get; set; }
        public DateTime TakenOn { get; set; }
        public bool IsFavourite { get; set; }
    }
}
