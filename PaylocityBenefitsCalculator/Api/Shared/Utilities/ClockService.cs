namespace Api.Shared.Utilities;

public class ClockService : IClockService
{
    public DateTime GetCurrentUtcDate() => DateTime.UtcNow.Date;
}