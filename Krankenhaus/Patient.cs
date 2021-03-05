using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Patient
    {
        public int PatientID { get; set; }
        public EventHandler VetEj { get; set; }
        public void VetEjHandler(object sender, EventArgs e)
        {

        }
    }
}
