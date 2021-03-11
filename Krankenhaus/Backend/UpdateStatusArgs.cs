using System;

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
