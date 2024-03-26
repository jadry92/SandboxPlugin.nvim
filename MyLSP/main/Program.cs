
using System;
using System.IO;
using System.Text;

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
    public static void Main()
    {
        string? basePath = Environment.GetEnvironmentVariable("PROJECTS_DIR");
        if (basePath == null)
        {
            throw new Exception("PROJECTS_DIR environment variable is not set");
        }

        Logger logger = new Logger($"{basePath}/SandboxPlugin.nvim/log.txt");
        logger.Log("the program has started");
        using (Stream stdin = Console.OpenStandardInput())
        {
            byte[] buffer = new byte[10];
            int bytes;
            StringBuilder builder = new StringBuilder();
            while ((bytes = stdin.Read(buffer, 0, buffer.Length)) > 0)
            {
                builder.Append(Encoding.ASCII.GetString(buffer, 0, bytes));
            }

            string result = builder.ToString();
            logger.Log(result);
        }

    }
}
