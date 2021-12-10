using System;

namespace Couple.Api.Infrastructure;

public interface IDateTimeService
{
    DateTime Now { get; }
}
