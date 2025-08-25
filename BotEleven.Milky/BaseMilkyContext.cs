using System.Diagnostics.CodeAnalysis;
using BotEleven.Milky.Transfer;

namespace BotEleven.Milky;

public abstract class BaseMilkyContext(string serverEndpoint, MilkyOptions? options = null) : BotContext
{
    private readonly Dialer _dialer = new(new Uri(serverEndpoint), options ?? MilkyOptions.Default);

    public override bool IsOpened => _dialer.Opened;
    public override void Open()
    {
        _dialer.Open();
    }

    public override void Close()
    {
        _dialer.Close();
    }

    public override async Task InvokeAction<T>(string actionName, T? parameters) where T : default
    {
        var actionResponse = await _dialer.InvokeAction<object>(actionName, parameters, CancellationToken.None);
        ThrowExceptionIfError(actionName, actionResponse);
    }

    public override async Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters) 
        where TReturn : default 
        where TParam : default
    {
        var actionResponse = await _dialer.InvokeAction<TReturn>(actionName, parameters, CancellationToken.None);
        ThrowExceptionIfError(actionName, actionResponse);

        return actionResponse.Data;
    }

    private static void ThrowExceptionIfError<T>(string actionName, [NotNull] ActionResponse<T>? response)
    {
        if (response == null)
        {
            throw new NullReferenceException("Null action response deserialized");
        }

        if (response.RetCode != 0)
        {
            throw new MilkyActionException(response.RetCode, actionName, response.Message ?? "<No message>");
        }
    }
}
