using BotOneOne.OneBot11.Connectivity;
using BotOneOne.OneBot11.Transfer.Dto;
using BotOneOne.OneBot11.Transfer.Packet;
using Newtonsoft.Json;

namespace BotOneOne.OneBot11;

public abstract class BaseOneBot11Context(IConnectionSource connectionSource, OneBot11Options? options)
    : BotContext
{
    private readonly OneBot11Options _options = options ?? OneBot11Options.Default;
    private readonly Dictionary<string, TaskCompletionSource<ActionResponsePacket>> _pendingRequests = [];
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;

    public override bool IsOpened => _workerTask != null;

    public override void Open()
    {
        if (_workerTask != null)
        {
            return;
        }

        _workerTask = Receiver(_cancellationTokenSource.Token);
    }

    public override void Close()
    {
        _cancellationTokenSource.Cancel();
        _cancellationTokenSource = new CancellationTokenSource();
        _workerTask = null;
    }

    private void HandleActionResponse(string packet)
    {
        var actionResponse = JsonConvert.DeserializeObject<ActionResponsePacket>(packet);
        if (actionResponse?.Echo == null)
        {
            return;
        }

        // handle packet by echo
        if (_pendingRequests.TryGetValue(actionResponse.Echo, out var source))
        {
            source.TrySetResult(actionResponse);
        }
    }

    protected abstract void HandleEvent(string eventType, string packet);

    private async Task Receiver(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var packet = await connectionSource.ReceiveTextAsync(cancellationToken);
                var baseResponse = JsonConvert.DeserializeObject<BaseIncomingPacket>(packet);

                if (baseResponse == null)
                {
                    continue;
                }

                if (baseResponse.IsEventPacket)
                {
                    HandleEvent(baseResponse.PostType!, packet);
                }
                else
                {
                    HandleActionResponse(packet);
                }
            }
            catch (Exception exception)
            {
                Utils.LogException(exception);
            }
        }
    }

    private async ValueTask<ActionResponsePacket> InvokeActionInternal(ActionRequestPacket requestPacket,
        CancellationToken cancellationToken)
    {
        var packetEcho = Guid.NewGuid().ToString();
        requestPacket.Echo = packetEcho;

        // prepare for receive
        var taskCompletionSource = new TaskCompletionSource<ActionResponsePacket>();
        _pendingRequests.Add(packetEcho, taskCompletionSource);
        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(), false);

        // send action packet
        await connectionSource.SendTextAsync(
            JsonConvert.SerializeObject(requestPacket), cancellationToken);

        try
        {
            // receive response
            var response = await taskCompletionSource.Task;
            return response;
        }
        catch (TaskCanceledException)
        {
            throw new Exception("Action invocation timeout");
        }
        finally
        {
            _pendingRequests.Remove(packetEcho);
        }
    }

    public override async Task InvokeAction<T>(string actionName, T? parameters) where T : default
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(_options.InvocationTimeout);
        await InvokeActionInternal(new ActionRequestPacket<T>
        {
            Action = actionName,
            Params = parameters
        }, cancellationTokenSource.Token);
    }

    public override async Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters)
        where TParam : default
        where TReturn : default
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(_options.InvocationTimeout);

        var response = await InvokeActionInternal(new ActionRequestPacket<TParam>
        {
            Action = actionName,
            Params = parameters
        }, cancellationTokenSource.Token);

        return response.ExtensionData.TryGetValue("data", out var value)
            ? value.ToObject<TReturn?>()
            : default;
    }
}