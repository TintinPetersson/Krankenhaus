using Krankenhaus.Backend;
using System;
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
                Console.Clear();
                Tick?.Invoke(this, EventArgs.Empty);
                tick++;
                await Task.Delay(time * 1000);
            }

            return;
        }

        public async Task StopTick()
        {
            keepTicking = false;
            await Task.Delay(1);
            TickerStop?.Invoke(this, new TimeTickArgs(startTime, tick));
            await logger.LogToFile("Ticker.txt", " ", false);
        }
    }
}
