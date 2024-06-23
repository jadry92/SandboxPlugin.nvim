
using LSP.Types;



namespace LSP.Analysis;

class State
{
    private Dictionary<string, string> Documents;

    public State()
    {
        Documents = new Dictionary<string, string>() { };
    }


    public Diagnostic[] GetDiagnosticsForFile(Notification<DidOpenTextDocumentParams> request)
    {
        string uri = request.@params.textDocument.URI;
        string text = request.@params.textDocument.text;
        this.Documents[uri] = text;

        Diagnostic[] diagnostics = [];

        const string VSCODE = "VS Code";
        const string NVIM = "Neovim";
        int row = 0;
        foreach (string line in text.Split('\n'))
        {

            if (line.Contains(VSCODE))
            {

                int idx = line.IndexOf(VSCODE);

                diagnostics.Append(new Diagnostic()
                {
                    range = this.LineRange(row, idx, idx + VSCODE.Length),
                    severity = 1,
                    source = "Commm sensce",
                    message = "use other stuff"

                });
            }
            else if (line.Contains(NVIM))
            {
                int idx = line.IndexOf(NVIM);

                diagnostics.Append(new Diagnostic()
                {
                    range = this.LineRange(row, idx, idx + NVIM.Length),
                    severity = 1,
                    source = "Commm sensce",
                    message = "grate"

                });
            }

            row++;
        }

        return diagnostics;
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

