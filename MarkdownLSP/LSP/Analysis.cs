
using LSP.Types;
using Serilog;
using System.Text;
using TrieDictionary;

namespace LSP.Analysis;


public class State
{
    private Dictionary<string, List<Dictionary<int, string>>> Documents;
    private LiteralDictionary LiteralDictionary;


    public State()
    {
        Documents = new Dictionary<string, List<Dictionary<int, string>>>() { };
        this.LiteralDictionary = new LiteralDictionary();
    }


    public List<Diagnostic> GetDiagnosticsForFile(Notification<DidOpenTextDocumentParams> request)
    {

        string uri = request.@params.textDocument.uri;
        string text = request.@params.textDocument.text;

        List<Diagnostic> diagnostics = new List<Diagnostic>();
        int row = 0;

        var listLines = new List<Dictionary<int, string>>();
        foreach (string line in text.Split('\n'))
        {
            StringBuilder sb = new StringBuilder();
            var dicText = new Dictionary<int, string>();
            for (int i = 0; i < line.Length; i++)
            {
                char letter = line[i];
                if (letter.Equals(" "))
                {
                    string word = sb.ToString();
                    Log.Debug(word);
                    int idx = word.Length - i;
                    dicText[idx] = word;
                    sb.Clear();
                    string meaning = this.LiteralDictionary.getDefinition(word);
                    if (meaning != null)
                    {
                        var dig = new Diagnostic()
                        {
                            range = this.LineRange(row, idx, idx + word.Length),
                            severity = 3,
                            source = "Dictionary",
                            message = meaning,
                        };
                        diagnostics.Add(dig);
                    }
                }
                else if (i == line.Length - 1)
                {
                    sb.Append(letter);
                    string word = sb.ToString();
                    Log.Debug(word);
                    int idx = word.Length - i;
                    dicText[idx] = word;
                    sb.Clear();
                    string meaning = this.LiteralDictionary.getDefinition(word);
                    var dig = new Diagnostic()
                    {
                        range = this.LineRange(row, idx, idx + word.Length),
                        severity = 3,
                        source = "Dictionary",
                        message = meaning,
                    };
                    diagnostics.Add(dig);
                }
                else
                {
                    sb.Append(letter);
                }
            }
            listLines.Add(dicText);
            row++;
        }

        this.Documents[uri] = listLines;

        return diagnostics;
    }

    public HoverResult Hover(Request<HoverParams> Request)
    {
        Position pos = Request.@params.position;
        string uri = Request.@params.textDocument.uri;


        int line = (int)pos.line;
        int character = (int)pos.character;

        Log.Debug(line.ToString());
        Dictionary<int, string> wordsDic = this.Documents[uri][line];

        var keys = wordsDic.Keys;

        string meaning = "";
        foreach (int key in keys)
        {
            Log.Debug(key.ToString());
            string word = wordsDic[key];
            Log.Debug(word);
            if (key <= character && key >= word.Length)
            {
                meaning = this.LiteralDictionary.getDefinition(word.ToLower());
            }
        }

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

