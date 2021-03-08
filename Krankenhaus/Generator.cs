using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Krankenhaus
{
    class Generator
    {
        private Queue queue;
        private Ticker ticker;
        EventHandler<EventArgs> StartClock;
        public Generator()
        {
            queue = new Queue();
            ticker = new Ticker();
        }

        public void Start()
        {
            ticker.Tick += ShowQueue;
            ticker.Tick += queue.OnTick();
            StartClock += StartTicker;

            MakePatients();

            StartClock?.Invoke(this, EventArgs.Empty);

            
            
        }

        public async void StartTicker(object sender, EventArgs e)
        {
            await Task.Run(() => ticker.Ticking());
        }

        public void ShowQueue(object sender, EventArgs e)
        {
            Console.WriteLine(queue.GetAllPatients());
        }

        public static Queue<Doctor> MakeDoctors()
        {
            Queue<Doctor> doctors = new Queue<Doctor>();

            for (int i = 0; i < 10; i++)
            {
                Doctor doctor = new Doctor();
                doctors.Enqueue(doctor);
                Thread.Sleep(250);
            }

            return doctors;
        }

        public void MakePatients()
        {
            for (int i = 0; i < 25; i++)
            {
                Patient patient = new Patient();
                queue.AddToQueue(patient);
                Thread.Sleep(250);
            }
        }
    }
}
