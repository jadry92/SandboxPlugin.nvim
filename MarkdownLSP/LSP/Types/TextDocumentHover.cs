
namespace LSP.Types;

public struct HoverParams
{
    public TextDocumentIdentifier textDocument { get; set; }
    public Position position { get; set; }
}

public struct HoverResult
{
    public string contents { get; set; }
}

