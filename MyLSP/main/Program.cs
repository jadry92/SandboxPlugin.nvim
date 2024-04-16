
using System.Text;
using System.Text.Json;
using InitializeLSP;

namespace Main;

public class Program
{
    public static void Main()
    {
        string? basePath = Environment.GetEnvironmentVariable("PROJECTS_DIR");
        if (basePath == null)
        {
            throw new Exception("PROJECTS_DIR environment variable is not set");
        }

        Logger logger = new Logger($"{basePath}/SandboxPlugin.nvim/log.txt");
        logger.Log("the program has started");
        Message message = new Message();
        using (Stream stdin = Console.OpenStandardInput())
        {
            while (true)
            {
                string requestStr = message.DecodeMessage(stdin);
                try
                {
                    Request? request = JsonSerializer.Deserialize<InitializeRequest>(requestStr);
                    logger.Log(request.method);
                    HandelRequest(request, logger, message);
                }
                catch (Exception e)
                {
                    logger.Log(e.Message);
                }
            }

        }

    }

    public static void HandelRequest(Request request, Logger logger, Message message)
    {
        if (request.method == "initialize")
        {
            int id = request.id ?? 0;
            var response = Parser.ParseInitializeRequest(id);
            string responseStr = JsonSerializer.Serialize(response);
            byte[] buffer = message.EncodeMessage(responseStr);
            Console.OpenStandardOutput().Write(buffer, 0, buffer.Length);
            logger.Log("initialize request has been handled");
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
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string chunk = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        string[] arrayHeader = chunk.Split("\r\n\r\n");
        string header = arrayHeader[0];
        string contentFirstChunk = arrayHeader[1];
        int contentLength = int.Parse(header.Split("Content-Length: ")[1]) - contentFirstChunk.Length;
        byte[] contentBuffer = new byte[contentLength];
        stream.Read(contentBuffer, 0, contentLength);
        string content = Encoding.ASCII.GetString(contentBuffer);
        content = contentFirstChunk + content;
        return content;
    }
}
public class Logger
{
    private string filePath;

    public Logger(string filePath)
    {
        this.filePath = filePath;
    }

    public void Log(string message)
    {
        using (StreamWriter streamWriter = new StreamWriter(this.filePath, false))
        {
            streamWriter.WriteLine($"{DateTime.Now}: {message}");
        }
    }
}
