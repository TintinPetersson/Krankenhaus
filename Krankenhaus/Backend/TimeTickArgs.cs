using System;

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
