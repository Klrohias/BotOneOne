using BotEleven.MessageFormat;

namespace BotEleven.Milky;

public abstract class BaseMilkyContext(string serverEndpoint, MilkyOptions? options = null) : BotContext
{
    private readonly Dialer _dialer = new(new Uri(serverEndpoint), options ?? MilkyOptions.Default);

    public override bool IsOpened { get; }
    public override void Open()
    {
        throw new NotImplementedException();
    }

    public override void Close()
    {
        throw new NotImplementedException();
    }

    public override Task InvokeAction<T>(string actionName, T? parameters) where T : default
    {
        throw new NotImplementedException();
    }

    public override Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters) where TReturn : default where TParam : default
    {
        throw new NotImplementedException();
    }
}