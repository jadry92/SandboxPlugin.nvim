
namespace LSP.Types;

public class HoverParams
{
    public TextDocumentPositionParams? TextDocumentPositionParams { get; set; }
}

public class HoverResult
{
    public string? Contents { get; set; }
}

