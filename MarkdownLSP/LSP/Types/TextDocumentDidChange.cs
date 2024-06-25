
namespace LSP.Types;


public struct DidChangeTextDocumentParams
{
    public VersionTextDocumentIdentifier? textDocument;
    public List<TextDocumentContentChangeEvent>? contentChange;
}

public struct TextDocumentContentChangeEvent
{
    public LSPRange? range { get; set; }
    public uint? rangeLength { get; set; }
    public string? text { get; set; }
}


