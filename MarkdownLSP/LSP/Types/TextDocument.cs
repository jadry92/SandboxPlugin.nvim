
namespace LSP.Types;

class TextDocument
{
    public string? URI { get; set; }
    public string? LanguageID { get; set; }
    public int? Version { get; set; }
    public string? Text { get; set; }
}

class TextDocumentIdentifier
{

    public string? URI { get; set; }
}

class VersionTextDocumentIdentifier
{
    public int? Version { get; set; }
}
