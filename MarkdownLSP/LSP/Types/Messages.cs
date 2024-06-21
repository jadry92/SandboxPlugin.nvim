
namespace LSP.Types;

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

