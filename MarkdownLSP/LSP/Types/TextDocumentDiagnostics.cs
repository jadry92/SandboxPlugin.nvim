
namespace LSP.Types;

public class PublishDiagnosticsParams
{
    public string? URI { get; set; }
    public Diagnostic[]? diagnostics { get; set; }

}

public class Diagnostic
{
    public LSPRange? range { get; set; }
    public int? severity { get; set; }
    public string? source { get; set; }
    public string? message { get; set; }
}

