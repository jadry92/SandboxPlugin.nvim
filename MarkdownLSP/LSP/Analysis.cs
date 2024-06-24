
using LSP.Types;
using Serilog;



namespace LSP.Analysis;

public class State
{
    private Dictionary<string, Dictionary<int, string>> Documents;

    public State()
    {
        Documents = new Dictionary<string, Dictionary<int, string>>() { };
    }


    public List<Diagnostic> GetDiagnosticsForFile(Notification<DidOpenTextDocumentParams> request)
    {
        if (request.@params == null || request.@params.textDocument == null)
        {
            throw new NullReferenceException("params or text document can't be null");
        }

        string? uri = request.@params.textDocument.uri;
        string? text = request.@params.textDocument.text;

        if (uri == null || text == null)
        {
            throw new NullReferenceException("uri or text can't be null");
        }

        List<Diagnostic> diagnostics = new List<Diagnostic>();

        const string VSCODE = "VS Code";
        const string NVIM = "Neovim";
        int row = 0;
        var dicText = new Dictionary<int, string>();
        foreach (string line in text.Split('\n'))
        {
            dicText[row] = line;
            if (line.Contains(VSCODE))
            {

                int idx = line.IndexOf(VSCODE);


                diagnostics.Add(new Diagnostic()
                {
                    range = this.LineRange(row, idx, idx + VSCODE.Length),
                    severity = 3,
                    source = "Commm sensce",
                    message = "use other stuff",
                });


            }
            else if (line.Contains(NVIM))
            {
                int idx = line.IndexOf(NVIM);

                diagnostics.Add(new Diagnostic()
                {
                    range = this.LineRange(row, idx, idx + NVIM.Length),
                    severity = 3,
                    source = "Commm sensce",
                    message = "grate",
                });
            }
            row++;
        }
        this.Documents[uri] = dicText;

        return diagnostics;
    }

    public HoverResult Hover(Request<HoverParams> Request)
    {
        Position pos = Request.@params.position;
        string? uri = Request.@params.textDocument.uri;

        if (pos == null || uri == null || pos.line == null)
        {
            throw new NullReferenceException("the hover request can't be null");
        }

        int row = (int)pos.line;

        string? line = this.Documents[uri][0];

        if (line == null)
        {
            throw new NullReferenceException("Hover: Line not found");
        }


        var result = new HoverResult()
        {
            contents = "Local Hover",
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

