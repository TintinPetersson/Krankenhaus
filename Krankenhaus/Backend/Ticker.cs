using Krankenhaus.Backend;
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
        public int tick;
        private string startTime;
        public event EventHandler Tick;
        public event EventHandler<TimeTickArgs> TickerStop;
        private bool keepTicking;
        private Logger logger;

        public Ticker()
        {
            logger = new Logger();
        }
        public async Task Ticking(int time)
        {
            keepTicking = true;

            startTime = DateTime.Now.ToString("T");

            while (keepTicking)
            {
                tick++;
                await Task.Delay(time * 1000);
                Tick?.Invoke(this, EventArgs.Empty);
            }

            return;
        }

        public async Task StopTick()
        {
            keepTicking = false;
            TickerStop?.Invoke(this, new TimeTickArgs(startTime, tick));
            await logger.LogToFile("Ticker.txt", " ", false);
        }
    }
}
