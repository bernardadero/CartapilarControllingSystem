using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CartapilarControllingSystem
{
    internal class Logger
    {
        private string filePath;

        public Logger(string filePath)
        {
            this.filePath = filePath;

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
        }

        public void Log(string message)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine($"{DateTime.Now} - {message}");
            }
        }
    }
}
