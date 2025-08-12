namespace BotOneOne;

public interface IProtocol
{
    public bool IsOpened { get; }
    public void Open();
    public void Close();
    public Task InvokeAction<T>(string actionName, T? parameters);
    public Task<TReturn?> InvokeAction<TReturn, TParam>(string actionName, TParam? parameters);
}
