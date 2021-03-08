using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    public class UpdateStatusArgs : EventArgs
    {
        public string Status { get; private set; }

        public UpdateStatusArgs(string status)
        {
            Status = status;
        }
    }
}
