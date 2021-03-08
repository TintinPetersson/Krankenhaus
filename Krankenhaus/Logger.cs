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
        public void LogToFileHandler(object sender, CustomEventArgs c)
        {
            using (StreamWriter writer = new StreamWriter("CurrentTime.txt", true))
            {
                writer.WriteLine(c.timeNow);
                writer.Flush();
            }
        }
    }
}
