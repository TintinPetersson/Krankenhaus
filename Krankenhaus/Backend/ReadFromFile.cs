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
        public void ReadInfo()
        {

        }

        public bool SessionExists()
        {
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
                return true;
            }
        }

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
