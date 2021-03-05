using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus
{
    public class Doctor
    {
        public int DoctorID { get; set; }
        public int MyProperty { get; set; }

        public EventHandler VetEj { get; set; }

        public void VetEjHandler(object sender, EventArgs e)
        {
            
        }
    }
}
