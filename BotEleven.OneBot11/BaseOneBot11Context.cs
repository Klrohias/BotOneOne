using System.Collections.Concurrent;
using System.Diagnostics;
using BotEleven.OneBot11.Connectivity;
using BotEleven.OneBot11.Internals;
using BotEleven.OneBot11.Transfer.Packet;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BotEleven.OneBot11;

public abstract class BaseOneBot11Context : BotContext
{
    private readonly SnowflakeId _snowflakeId;
    private readonly OneBot11Options _options;
    private readonly ConcurrentDictionary<long, TaskCompletionSource<ActionResponsePacket>> _pendingRequests = [];
    private readonly IConnectionSource _connectionSource;
    private readonly Lock _lock = new();
    
    private CancellationTokenSource _cancellationTokenSource = new();
    private Task? _workerTask;

    protected BaseOneBot11Context(IConnectionSource connectionSource, OneBot11Options? options)
    {
        _connectionSource = connectionSource;
        _options = options ?? OneBot11Options.Default;
        _snowflakeId = new SnowflakeId(_options.WorkerId);
    }

    public override bool IsOpened => _workerTask != null;

    public override void Open()
    {
        lock (_lock)
        {
            if (_workerTask != null)
            {
                return;
            }

            _workerTask = Receiver(_cancellationTokenSource.Token);
        }
    }

    public override void Close()
    {
        lock (_lock)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource = new CancellationTokenSource();
            _workerTask = null;
        }
    }

    private void HandleActionResponse(JToken packet)
    {
        var actionResponse = packet.ToObject<ActionResponsePacket>();
        if (actionResponse?.Echo == null)
        {
            return;
        }

        // handle packet by echo
        if (!long.TryParse(actionResponse.Echo, out var echo))
        {
            // the action is not sent by BotEleven, drop
            return;
        }
        
        if (_pendingRequests.TryGetValue(echo, out var source))
        {
            source.TrySetResult(actionResponse);
        }
    }

    protected abstract void HandleEvent(string eventType, JToken packet);

    private async Task Receiver(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // read next packet and parse
                var jsonPacket = JToken.Parse(await _connectionSource.ReceiveTextAsync(cancellationToken));
                var baseResponse = jsonPacket.ToObject<BaseIncomingPacket>();

                if (baseResponse == null)
                {
                    continue;
                }

                // check the packet type and dispatch
                if (baseResponse.IsEventPacket)
                {
                    HandleEvent(baseResponse.PostType!, jsonPacket);
                }
                else
                {
                    HandleActionResponse(jsonPacket);
                }
            }
            catch (Exception exception)
            {
                Logger.LogException(exception);
            }
        }
    }

    private async ValueTask<ActionResponsePacket> InvokeActionInternal(ActionRequestPacket requestPacket,
        CancellationToken cancellationToken)
    {
        var packetEcho = _snowflakeId.Next();
        requestPacket.Echo = packetEcho.ToString();

        // prepare for receive
        var taskCompletionSource = new TaskCompletionSource<ActionResponsePacket>();
        if (!_pendingRequests.TryAdd(packetEcho, taskCompletionSource))
        {
            throw new UnreachableException();
        }
        
        cancellationToken.Register(() => taskCompletionSource.TrySetCanceled(), false);

        // send action packet
        await _connectionSource.SendTextAsync(
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
            _pendingRequests.Remove(packetEcho, out _);
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