﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Ticker
    {
        public event EventHandler Tick;
        private bool keepTicking;
        public async Task Ticking(int time)
        {
            keepTicking = true;

            while (keepTicking)
            {
                await Task.Delay(time * 1000);
                Tick?.Invoke(this, EventArgs.Empty);
            }

            return;
        }

        public void StopTick()
        {
            keepTicking = false;
        }
    }
}
