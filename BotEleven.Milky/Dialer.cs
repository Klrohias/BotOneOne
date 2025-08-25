namespace BotEleven.Milky;

internal class Dialer(Uri endpoint, MilkyOptions options)
{
    private readonly Uri _endpoint = endpoint;
    private readonly MilkyOptions _options = options;
    private readonly HttpClient _client = new();

    private bool _opened = false;
    
}