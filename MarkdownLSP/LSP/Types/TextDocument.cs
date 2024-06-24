
namespace LSP.Types;

public class TextDocumentPositionParams
{
    public TextDocumentIdentifier? textDocument { get; set; }
    public Position? position { get; set; }
}

public class TextDocumentIdentifier
{
    public string? uri { get; set; }
}

public class VersionTextDocumentIdentifier
{
    public int? version { get; set; }
}


public class TextDocumentItem
{
    public string? uri { get; set; }
    public string? languageId { get; set; }
    public int? version { get; set; }
    public string? text { get; set; }
}

public class Position
{
    public uint? line { get; set; }
    public uint? character { get; set; }
}

public class LSPRange
{
    public Position? start { get; set; }
    public Position? end { get; set; }
}

public class TextEdit
{
    public LSPRange? range { get; set; }
    public string? newText { get; set; }
}

public class WorkspaceEdite
{
    public Dictionary<string, TextEdit>? changes { get; set; }
}


