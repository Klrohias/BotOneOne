namespace BotEleven.OneBot11.Internals;

public class SnowflakeId(short workerId = 1)
{
    private const long WorkerIdMask = 1023L;
    private const long SequenceIdMask = 4095L;
    
    private readonly long _workerId = WorkerIdMask & workerId;
    private long _sequenceId = 0;
    
    public long Next()
    {
        // var nextSequenceId = Interlocked.Add(ref _sequenceId, 1);
        // if (nextSequenceId > SequenceIdMask)
        // {
        //     Interlocked.Exchange(ref _sequenceId, 0);
        //     // wait for next millisecond
        // }
        throw new NotImplementedException();
    }
}