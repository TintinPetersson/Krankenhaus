using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Logger
    {
        private static ConcurrentQueue<Tuple<string, string>> buffer;

        public Logger()
        {
            Generator.UpdateStatus += LogToFileHandler;
            buffer = new ConcurrentQueue<Tuple<string, string>>();
        }
        public void LogToFileHandler(object sender, UpdateStatusArgs e)
        {
            using (StreamWriter writer = new StreamWriter("KrankenhausReport.txt", true))
            {
                writer.WriteLine(e.Status);
                writer.Flush();
            }
        }

        public async Task LogToFile(string path, string text, bool append)
        {

            using (StreamWriter writer = new StreamWriter(path, append))
            {
                await writer.WriteLineAsync(text);
                await writer.FlushAsync();
            }


        }

        public async Task AddToBuffer(string path, string text)
        {
            var add = new Tuple<string, string>(path, text);
            buffer.Enqueue(add);

            while (buffer.Count != 0)
            {
                if (buffer.TryDequeue(out Tuple<string, string> next))
                {
                    await LogToFile(next.Item1, next.Item2, true);
                }
            }

        }

    }
}
