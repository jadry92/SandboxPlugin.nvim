
using System.Text;
using System.Text.Json;

namespace Main;

public class Program
{

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
    public class Message
    {
        public string EncodeMessage(string message)
        {
            return message;
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
                    logger.Log(requestStr);
                    Request request = JsonSerializer.Deserialize<Request>(requestStr);
                    logger.Log(request.method);
                }
                catch (Exception e)
                {
                    logger.Log(e.Message);
                }
            }

        }

    }
    class Request
    {
        public string method { get; set; }
        public int id { get; set; }
    }
}
