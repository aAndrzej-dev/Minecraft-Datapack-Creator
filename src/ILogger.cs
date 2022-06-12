using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftDatapackCreator
{
    public interface ILogger
    {
        void Debug(string message);
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
    }
    internal class Logger : ILogger
    {
        private readonly string filename;
        private readonly string instanceId;
        public Logger(string filename, string instanceId)
        {
            DirectoryInfo di = new DirectoryInfo(Path.GetDirectoryName(filename)!);
            if (!di.Exists)
                di.Create();
            this.filename = filename;
            this.instanceId = instanceId;
        }

        public void Error(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][ERROR]\t{message.Replace("\n", "\\n").Replace("\r", "\\r")}");
        public void Fatal(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][FATAL]\t{message.Replace("\n", "\\n").Replace("\r", "\\r")}");
        public void Info(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][INFO]\t{message.Replace("\n", "\\n").Replace("\r", "\\r")}");
        public void Debug(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][DEBUG]\t{message.Replace("\n", "\\n").Replace("\r", "\\r")}");
        public void Warn(string message) => WriteMessage($"[{instanceId}][{DateTime.Now:dd.MM.yyyy HH:mm:ss}][WARN]\t{message.Replace("\n", "\\n").Replace("\r", "\\r")}");


        private void WriteMessage(string message) => File.AppendAllText(filename, "\n" + message);
    }
}
