
using LSP.Types;
using Serilog;
using System.Text;
using TrieDictionary;

namespace LSP.Analysis;


public class State
{
    private Dictionary<string, List<string>> Documents;
    private LiteralDictionary LiteralDictionary;


    public State()
    {
        Documents = new Dictionary<string, List<string>>() { };
        this.LiteralDictionary = new LiteralDictionary();
    }


    public List<Diagnostic> GetDiagnosticsForFile(Notification<DidOpenTextDocumentParams> notification)
    {

        string uri = notification.@params.textDocument.uri;
        string text = notification.@params.textDocument.text;


        List<Diagnostic> diagnostics = new List<Diagnostic>();

        var listLines = new List<string>();
        foreach (string line in text.Split('\n'))
        {
            listLines.Add(line);
        }

        this.Documents[uri] = listLines;

        return diagnostics;
    }


    public void OnChange(Notification<DidChangeTextDocumentParams> notification)
    {

        string uri = notification.@params.textDocument.uri;


        foreach (var changes in notification.@params.contentChanges)
        {
            string text = changes.text;

            var listLines = new List<string>();
            foreach (string line in text.Split('\n'))
            {
                listLines.Add(line);
            }

            this.Documents[uri] = listLines;
        }
    }

    public HoverResult Hover(Request<HoverParams> Request)
    {
        Position pos = Request.@params.position;
        string uri = Request.@params.textDocument.uri;


        int line = (int)pos.line;
        int character = (int)pos.character;

        string textLine = this.Documents[uri][line];

        string word = this.GetWordAtIndex(textLine, character);

        string meaning = "";
        if (!string.IsNullOrEmpty(word))
        {
            meaning = this.LiteralDictionary.getDefinition(word);
        }

        var result = new HoverResult()
        {
            contents = meaning,
        };

        return result;

    }

    private string GetWordAtIndex(string text, int index)
    {
        if (index < 0 || index >= text.Length || char.IsWhiteSpace(text[index]))
        {
            return "";
        }

        int start = index;
        while (start > 0 && !char.IsWhiteSpace(text[start - 1]))
        {
            start--;
        }

        int end = index;
        while (end < text.Length && !char.IsWhiteSpace(text[end]))
        {
            end++;
        }

        return text.Substring(start, end - start);
    }

    private LSPRange LineRange(int line, int start, int end)
    {

        return new LSPRange()
        {
            start = new Position()
            {
                line = ((uint)line),
                character = ((uint)start),
            },
            end = new Position()
            {
                line = ((uint)line),
                character = ((uint)end),
            }
        };
    }

}

