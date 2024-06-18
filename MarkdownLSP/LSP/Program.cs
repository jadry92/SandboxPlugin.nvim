
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


        Message message = new Message();
        using (Stream stdin = Console.OpenStandardInput())
        {
            Request? request;
            while (keepRunning)
            {
                try
                {
                    string requestStr = message.DecodeMessage(stdin);
                    if (!string.IsNullOrEmpty(requestStr))
                    {
                        Log.Debug(requestStr);
                        request = JsonSerializer.Deserialize<InitializeRequest>(requestStr);
                        if (request != null && request.method != null)
                        {
                            Log.Debug(request.method);
                            HandelRequest(request, message);
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

    public static void HandelRequest(Request request, Message message)
    {
        if (request.method == "initialize")
        {
            int id = request.id ?? 0;
            var response = Parser.ParseInitializeRequest(id);
            string responseStr = JsonSerializer.Serialize(response);
            byte[] buffer = message.EncodeMessage(responseStr);
            Console.OpenStandardOutput().Write(buffer, 0, buffer.Length);
            Log.Debug("initialize request has been handled");
        }
        else if (request.method == "shutdown")
        {
            Log.Debug("Closing LSP");
            Environment.Exit(0);
        }
    }
}
public class Message
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
