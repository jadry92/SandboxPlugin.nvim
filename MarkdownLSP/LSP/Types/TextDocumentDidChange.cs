
namespace LSP.Types;


public struct DidChangeTextDocumentParams
{
    public VersionTextDocumentIdentifier textDocument { get; set; }
    public List<TextDocumentContentChangeEvent> contentChanges { get; set; }
}

public struct TextDocumentContentChangeEvent
{
    public string text { get; set; }
}

