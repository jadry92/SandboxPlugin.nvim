
using System.Text;
using System.Text.Json;
using InitializeLSP;
using Serilog;
using TrieDictionary;


namespace Main;

public class Program
{
    public static void Main()
    {
        bool keepRunning = true;

        Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e)
        {
            e.Cancel = true;
            keepRunning = false;
        };

        string? basePath = Environment.GetEnvironmentVariable("PROJECTS_DIR");
        if (basePath == null)
        {
            throw new Exception("PROJECTS_DIR environment variable is not set");
        }

        string _filePath = $"{basePath}/MarkdownLSP/log.txt";
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File(_filePath)
            .CreateLogger();

        Log.Debug("The program has started");

        try
        {
            var literalDictionary = new LiteralDictionary();
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
        }


        Encoder encoder = new Encoder();
        using (Stream stdin = Console.OpenStandardInput())
        {
            Request? request;
            string requestStr = "";
            while (keepRunning)
            {
                try
                {
                    requestStr = encoder.DecodeMessage(stdin);
                    if (!string.IsNullOrEmpty(requestStr))
                    {
                        request = JsonSerializer.Deserialize<Request>(requestStr);
                        if (request != null && request.method != null)
                        {
                            HandelRequest(request, encoder);
                        }
                        else
                        {
                            Log.Error($"Request Error: {request}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(e.Message);
                }
            }
        }
    }

    public static void HandelRequest(Request request, Encoder encoder)
    {
        switch (request.method)
        {
            case "initialize":
                int id = request.id ?? 0;
                var response = Parser.ParseInitializeRequest(id);
                string responseStr = JsonSerializer.Serialize(response);
                byte[] buffer = encoder.EncodeMessage(responseStr);
                Console.OpenStandardOutput().Write(buffer, 0, buffer.Length);

                Log.Debug("initialize request has been handled");

                break;
            case "textDocument/didOpen":


                Log.Debug("To be implemented Open");

                break;
            case "textDocument/didChange":
                Log.Debug("To be implemented Change");

                break;
            case "textDocument/completion":
                Log.Debug("To be implemented Competition");

                break;
            case "textDocument/codeAction":
                Log.Debug("To be implemented code Action");

                break;
            case "textDocument/definition":
                Log.Debug("To be implemented definition");

                break;
            case "textDocument/hover":
                Log.Debug("To be implemented hover");

                break;

            case "shutdown":

                Log.Debug("Closing LSP");
                Environment.Exit(0);
                break;

            default:
                break;
        }
    }
}

public class Parcer : wq<T>
{
    private Encoder encoder;

    public Comunication()
    {
        encoder = new Encoder();
    }

    public void SendRequest(T Response)
    {
        string responseStr = JsonSerializer.Serialize(Response);
        byte[] buffer = encoder.EncodeMessage(responseStr);
        Console.OpenStandardOutput().Write(buffer, 0, buffer.Length);
    }
}


public class Encoder
{
    public byte[] EncodeMessage(string message)
    {
        string content = $"Content-Length: {message.Length}\r\n\r\n{message}";
        byte[] buffer = Encoding.ASCII.GetBytes(content);
        return buffer;
    }
    public string DecodeMessage(Stream stream)
    {
        // Get the header
        const string headerStr = "Content-Length: ";
        byte[] buffer = new byte[headerStr.Length];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string headerRead = Encoding.ASCII.GetString(buffer, 0, bytesRead);

        if (string.IsNullOrEmpty(headerRead))
        {
            return "";
        }
        if (!headerStr.Equals(headerRead))
        {
            throw new Exception($" Wronhg header {headerRead}");
        }

        var sizeContent = new StringBuilder();
        byte[] B = new byte[1];
        while (true)
        {
            stream.Read(B, 0, 1);
            string Bstr = Encoding.ASCII.GetString(B, 0, 1);
            if (Bstr.Equals("\r"))
            {
                byte[] others = new byte[3];
                stream.Read(others, 0, 3);
                break;
            }
            else
            {
                sizeContent.Append(Bstr);
            }
        }
        int contentLength = int.Parse(sizeContent.ToString());

        // string[] arrayHeader = chunk.Split("\r\n\r\n");
        // string header = arrayHeader[0];
        // string contentFirstChunk = arrayHeader[1];
        // int contentLength = int.Parse(header.Split("Content-Length: ")[1]) - contentFirstChunk.Length;
        byte[] contentBuffer = new byte[contentLength];
        stream.Read(contentBuffer, 0, contentLength);
        string content = Encoding.ASCII.GetString(contentBuffer);
        return content;
    }
}
