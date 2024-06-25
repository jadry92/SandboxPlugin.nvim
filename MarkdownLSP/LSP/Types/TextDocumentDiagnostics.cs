
namespace LSP.Types;

public struct PublishDiagnosticsParams
{
    public string uri { get; set; }
    public List<Diagnostic> diagnostics { get; set; }

}

public struct Diagnostic
{
    public LSPRange range { get; set; }
    public int severity { get; set; }
    public string source { get; set; }
    public string message { get; set; }
}

