using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Krankenhaus
{
    class Logger
    {
        public async Task LogToFile(string path, string text, bool append)
        {
            using (StreamWriter writer = new StreamWriter(path, append))
            {
                await writer.WriteLineAsync(text);
                await writer.FlushAsync();
            }
        }
    }
}
