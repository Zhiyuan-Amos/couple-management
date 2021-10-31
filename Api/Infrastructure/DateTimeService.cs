using System;

namespace Couple.Api.Infrastructure
{
    public class DateTimeService : IDateTimeService
    {
        public DateTime Now => DateTime.Now;
    }
}
