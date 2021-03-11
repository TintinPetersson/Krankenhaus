using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Krankenhaus.Backend
{
    class ReadFromFile
    {
        /// <summary>
        /// Checks if an active simulation exists
        /// </summary>
        /// <param name="ticks">Shows which tick the simulation was on in the last session</param>
        /// <returns>a boolean</returns>
        public bool SessionExists(out int ticks)
        {
            ticks = 0;
            if (!File.Exists("Ticker.txt"))
            {
                return false;
            }
            StreamReader reader = new StreamReader("Ticker.txt");
            using (reader)
            {
                var line = reader.ReadLine();
                if (String.IsNullOrWhiteSpace(line))
                {
                    return false;
                }
                ticks = int.Parse(line);
                return true;
            }
        }

        /// <summary>
        /// Retrieves data from given file path and returns them as an IEnumerable
        /// </summary>
        /// <param name="path">File path to retrieve data from</param>
        /// <returns>An IEnumerable<Person></returns>
        public IEnumerable<Person> GetPeopleList(string path)
        {
            StreamReader reader = new StreamReader(path);
            using (reader)
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    var properties = line.Split('#');

                    if (properties[0] == "Doctor")
                    {
                        var doc = new Doctor(properties[1], int.Parse(properties[2]));
                        doc.Fatigue = int.Parse(properties[3]);
                        yield return doc;
                    }
                    else if (properties[0] == "Patient")
                    {
                        var pat = new Patient(properties[1], int.Parse(properties[2]), properties[3], DateTime.Parse(properties[5]), DateTime.Parse(properties[6]));
                        yield return pat;
                    }
                }
            }
        }

    }
}
