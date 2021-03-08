using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Logger
    {
        public Logger()
        {
            Generator.UpdateStatus += LogToFileHandler;
        }
        public void LogToFileHandler(object sender, UpdateStatusArgs e)
        {
            using (StreamWriter writer = new StreamWriter("KrankenhausReport.txt", true))
            {
                writer.WriteLine(e.Status);
                writer.Flush();
            }
        }

    }
}
