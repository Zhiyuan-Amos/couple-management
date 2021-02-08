using System;

namespace Api.Infrastructure
{
    public interface IDateTimeService
    {
        DateTime Now { get; }
    }
}
