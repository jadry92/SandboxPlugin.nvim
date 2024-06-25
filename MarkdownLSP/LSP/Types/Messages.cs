
namespace LSP.Types;

public struct Request<T>
{
    public string method { get; set; }
    public int id { get; set; }
    public string jsonrpc { get; set; }
    public T @params { get; set; }
}

public struct Response<T>
{
    public string jsonrpc { get; set; }
    public int id { get; set; }
    // Response
    public T result { get; set; }
    // Error
}

public struct Notification<T>
{
    public string jsonrpc { get; set; }
    public string method { get; set; }
    public T @params { get; set; }
}


