
namespace LSP.Types;

public class HoverParams : TextDocumentPositionParams
{
}

public class HoverResult
{
    public string? contents { get; set; }
}

