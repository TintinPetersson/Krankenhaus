using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Krankenhaus.Backend
{
    public enum Title { Doctor, Patient} 
    public class Person
    {
        public Title Title { get; private set; }

        public Person(Title title)
        {
            this.Title = title;
        }

    }
}
