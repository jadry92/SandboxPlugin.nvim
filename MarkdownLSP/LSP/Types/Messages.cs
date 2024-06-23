
namespace LSP.Types;

public class Request<T>
{
    public string? method { get; set; }
    public int? id { get; set; }
    public string? jsonrpc { get; set; }
    public T? @params { get; set; }
}

public class Response<T>
{
    public string? jsonrpc { get; set; }
    public int? id { get; set; }
    // Response
    public T? result { get; set; }
    // Error
}

public class Notification<T>
{
    public string? jsonrpc { get; set; }
    public string? method { get; set; }
    public T? @params { get; set; }
}


