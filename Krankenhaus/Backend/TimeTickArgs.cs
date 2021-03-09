using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus.Backend
{
    class TimeTickArgs : EventArgs
    {
        public string StartTime { get; set; }

        public int Ticks { get; set; }

        public TimeTickArgs(string s, int i)
        {
            StartTime = s;
            Ticks = i;
        }

    }
}
