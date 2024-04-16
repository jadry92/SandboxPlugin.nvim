
namespace InitializeLSP;
// Basic LSP Initialize Request and Response
public class Request
{
    public string? method { get; set; }
    public int? id { get; set; }
    public string? jsonrpc { get; set; }
}

public class Response
{
    public string? jsonrpc { get; set; }
    public int? id { get; set; }
}

// Request

public class InitializeRequest : Request
{
    public InitializeRequestParams? @params { get; set; }
}

public class InitializeRequestParams
{
    public ClientInfo? clientInfo { get; set; }
}

public class ClientInfo
{
    public string? name { get; set; }
    public string? version { get; set; }
}

// Response

public class InitializeResponse : Response
{
    public InitializeResult? result { get; set; }
}

public class InitializeResult
{
    public ServerCapabilities? capabilities { get; set; }
    public ServerInfo? serverInfo { get; set; }
}

public class ServerInfo
{
    public string? name { get; set; }
    public string? version { get; set; }
}

public class ServerCapabilities
{
    public int? textDocumentSync { get; set; }
    public bool? hoverProvider { get; set; }
    public bool? codeActionProvider { get; set; }
    public bool? definitionProvider { get; set; }
    public Dictionary<string, object>? completionProvider { get; set; }
}


class Parser
{
    static public InitializeResponse? ParseInitializeRequest(int id)
    {
        InitializeResponse request = new InitializeResponse()
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
                    completionProvider = new Dictionary<string, object>() { }
                },
                serverInfo = new ServerInfo() { name = "myLSP", version = "1.0soasne" }
            }
        };
        return request;
    }
}
