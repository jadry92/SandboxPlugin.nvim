
namespace LSP.Types;


class DidChangeTextDocumentParams
{
    public VersionTextDocumentIdentifier? textDocument;
    public TextDocumentContentChangeEvent[]? contentChange;
}

class TextDocumentContentChangeEvent
{
    public LSPRange? range { get; set; }
    public uint? rangeLength { get; set; }
    public string? text { get; set; }
}


