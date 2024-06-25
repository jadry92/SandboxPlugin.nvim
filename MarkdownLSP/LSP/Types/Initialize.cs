
namespace LSP.Types;

public struct InitializeRequestParams
{
    public ClientInfo clientInfo { get; set; }
}

public struct ClientInfo
{
    public string name { get; set; }
    public string version { get; set; }
}

// Response


public struct InitializeResult
{
    public ServerCapabilities capabilities { get; set; }
    public ServerInfo serverInfo { get; set; }
}

public struct ServerInfo
{
    public string name { get; set; }
    public string version { get; set; }
}

public struct ServerCapabilities
{
    public int textDocumentSync { get; set; }
    public bool hoverProvider { get; set; }
    public bool codeActionProvider { get; set; }
    public bool definitionProvider { get; set; }
    public Dictionary<string, object> completionProvider { get; set; }
}


class Generator
{
    static public Response<InitializeResult> ParseInitializeRequest(int id)
    {
        var request = new Response<InitializeResult>()
        {
            jsonrpc = "2.0",
            id = id,
            result = new InitializeResult()
            {
                capabilities = new ServerCapabilities()
                {
                    textDocumentSync = 1,
                    hoverProvider = true,
                    codeActionProvider = true,
                    definitionProvider = true,
                    completionProvider = new Dictionary<string, object>() { },
                },
                serverInfo = new ServerInfo() { name = "MarkdowbnLSP", version = "1.0-MarkdownLSP" }
            }
        };
        return request;
    }
}
