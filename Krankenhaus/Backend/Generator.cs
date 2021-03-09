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
        private Sanatorium sanatorium;
        private IVA iva;
        internal static List<Patient> afterlife;
        internal static List<Patient> survivors;
        internal static Queue<Doctor> doctorsQueue;
        private Ticker ticker;
        private Frontend menu;
        private Logger logger;
        EventHandler<EventArgs> StartClock;
        public static EventHandler<UpdateStatusArgs> UpdateStatus;
        

        public Generator()
        {
            queue = new Queue();
            doctorsQueue = new Queue<Doctor>();
            ticker = new Ticker();
            sanatorium = new Sanatorium();
            iva = new IVA();
            afterlife = new List<Patient>();
            survivors = new List<Patient>();
            menu = new Frontend();
            logger = new Logger();
        }

        public void Start()
        {
            ticker.Tick += queue.OnTick;
            StartClock += StartTicker;
            ticker.Tick += StatusReport;
            ticker.Tick += FillHospital;
            ticker.Tick += CheckIfPatientsExist;
            ticker.Tick += sanatorium.OnTick;
            ticker.Tick += iva.OnTick;
            ticker.TickerStop += menu.DisplayResult;

            MakePatients();
            //Patients

            ShowQueue();

            doctorsQueue = MakeDoctors();
            //Queue of doctors


            StartClock?.Invoke(this, EventArgs.Empty);


        }

        public async void StartTicker(object sender, EventArgs e)
        {
            int time = menu.Menu();
            await Task.Run(() => ticker.Ticking(time));
        }

        public void ShowQueue()
        {
            Console.WriteLine(queue.GetAllPatients());
        }

        public void CheckIfPatientsExist(object sender, EventArgs e)
        {
            if (queue.Length == 0)
            {
                if (sanatorium.OccupiedBeds == 0 && iva.OccupiedBeds == 0)
                {
                    ticker.StopTick();
                }
            }
        }

        public void StatusReport(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Tick: {ticker.tick}");
            sb.AppendLine($"Queue: {queue.Length} patients");
            sb.AppendLine($"Sanatorium: {sanatorium.OccupiedBeds} patients");
            sb.AppendLine($"IVA: {iva.OccupiedBeds} patients");
            sb.AppendLine($"Doctor Present: {iva.DoctorPresent}");
            sb.AppendLine($"Afterlife: {afterlife.Count}");
            sb.AppendLine($"Survivors: {survivors.Count}");

            UpdateStatus?.Invoke(this, new UpdateStatusArgs(sb.ToString()));


        }

        public void FillHospital(object sender, EventArgs e)
        {
            while (queue.Length != 0)
            {
                if (!queue.Peek().IsAlive)
                {
                    afterlife.Add(queue.GetNextPatient());
                    continue;
                }

                if (!sanatorium.IsFull)
                {
                    sanatorium.CheckIn(queue.GetNextPatient());
                }
                else if (!iva.IsFull)
                {
                    iva.CheckIn(queue.GetNextPatient());
                }
                else
                {
                    break;
                }
            }
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
