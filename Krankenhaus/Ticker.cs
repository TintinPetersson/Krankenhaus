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
        private bool keepTicking;
        public async Task Ticking()
        {
            keepTicking = true;

            while (keepTicking)
            {
                await Task.Delay(5000);
                Tick?.Invoke(this, new CustomEventArgs());
            }
        }

        public void StopTick()
        {
            keepTicking = false;
        }
    }
}
