using System;

namespace Krankenhaus
{
    class Program
    {
        static void Main(string[] args)
        {
            //bara iva clearar
            Generator generator = new Generator();
            generator.Start();

            Console.ReadLine();
        }
    }
}
