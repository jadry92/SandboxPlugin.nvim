
using LSP.Types;
using Serilog;
using System.Text;
using TrieDictionary;

namespace LSP.Analysis;

public class State
{
    private Dictionary<string, Dictionary<int, string>> Documents;
    private LiteralDictionary LiteralDictionary;


    public State()
    {
        Documents = new Dictionary<string, Dictionary<int, string>>() { };
        this.LiteralDictionary = new LiteralDictionary();
    }


    public List<Diagnostic> GetDiagnosticsForFile(Notification<DidOpenTextDocumentParams> request)
    {

        string uri = request.@params.textDocument.uri;
        string text = request.@params.textDocument.text;

        List<Diagnostic> diagnostics = new List<Diagnostic>();

        int row = 0;
        var dicText = new Dictionary<int, string>();
        foreach (string line in text.Split('\n'))
        {
            dicText[row] = line;
            row++;

        }
        this.Documents[uri] = dicText;

        return diagnostics;
    }

    public HoverResult Hover(Request<HoverParams> Request)
    {
        Position pos = Request.@params.position;
        string uri = Request.@params.textDocument.uri;


        int row = (int)pos.line;

        string line = this.Documents[uri][row];

        StringBuilder word = new StringBuilder();
        for (int i = (int)pos.character; i < line.Length; i++)
        {
            char letter = line[i];
            if (letter.Equals(" ") || letter.Equals("\n"))
            {
                break;
            }
            else
            {
                word.Append(letter);
            }
        }

        Log.Debug(word.ToString());

        string meaning = this.LiteralDictionary.getDefinition(word.ToString());

        var result = new HoverResult()
        {
            contents = meaning,
        };

        return result;

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

