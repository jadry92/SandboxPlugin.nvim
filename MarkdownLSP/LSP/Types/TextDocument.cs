
namespace LSP.Types;

public struct TextDocumentPositionParams
{
    public TextDocumentIdentifier textDocument { get; set; }
    public Position position { get; set; }
}

public struct TextDocumentIdentifier
{
    public string uri { get; set; }
}

public struct VersionTextDocumentIdentifier
{
    public string uri { get; set; }
    public uint version { get; set; }
}


public struct TextDocumentItem
{
    public string uri { get; set; }
    public string languageId { get; set; }
    public int version { get; set; }
    public string text { get; set; }
}

public struct Position
{
    public uint line { get; set; }
    public uint character { get; set; }
}

public struct LSPRange
{
    public Position start { get; set; }
    public Position end { get; set; }
}

public struct TextEdit
{
    public LSPRange range { get; set; }
    public string newText { get; set; }
}

public struct WorkspaceEdite
{
    public Dictionary<string, TextEdit> changes { get; set; }
}


