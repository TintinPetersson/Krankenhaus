using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Ticker
    {
        public event EventHandler<CustomEventArgs> Tick;
        public void Ticking()
        {
            while (true)
            {
                string s = DateTime.Now.ToString("T");
                Tick?.Invoke(this, new CustomEventArgs(s));
                Thread.Sleep(1000);
            }
        }
    }
}
