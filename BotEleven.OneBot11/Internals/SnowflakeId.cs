namespace BotEleven.OneBot11.Internals;

public class SnowflakeId(short workerId = 1)
{
    private const long WorkerIdMask = 1023L;
    private const long SequenceIdMask = 4095L;
    private const long TimestampMask = 2199023255551L;

    private readonly long _workerId = WorkerIdMask & workerId;
    private long _sequenceId;
    private long _lastTimestamp = GetCurrentTimestamp();
    
    private readonly Lock _sync = new();

    public long Next()
    {
        lock (_sync)
        {
            var currentTimestamp = GetCurrentTimestamp();
            if (currentTimestamp < _lastTimestamp)
            {
                // clock backwards
                WaitForNextMillisecond();
            } else if (currentTimestamp == _lastTimestamp)
            {
                // roll the sequence id
                if (_sequenceId++ >= SequenceIdMask)
                {
                    WaitForNextMillisecond();
                    currentTimestamp = GetCurrentTimestamp();
                }
            }

            if (currentTimestamp > _lastTimestamp)
            {
                // to the future?
                _sequenceId = 0;
                _lastTimestamp = currentTimestamp;
            }

            return (currentTimestamp & TimestampMask << 22)
                | (_workerId << 12)
                | _sequenceId;
        }
    }


    private void WaitForNextMillisecond()
    {
        while (GetCurrentTimestamp() <= _lastTimestamp)
        {
        }

        _lastTimestamp = GetCurrentTimestamp();
    }

    private static long GetCurrentTimestamp() => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
}