using System;
using System.Threading;

namespace Couriers_Upgrade
{
    class Time
    {
        public static DateTime Current_Time { get; private set; }
        private int Delay = 500;
        public Time()
        {
            Current_Time = DateTime.Now;
        }
        public static DateTime Add_Random_Time(DateTime date)
        {
            Random rnd = new Random();
            Thread.Sleep(30);
            return date.AddMinutes(rnd.Next(70, 100));
        }
        public static DateTime Subtract_Random_Time(DateTime date)
        {
            Random rnd = new Random();
            Thread.Sleep(30);
            return date.AddMinutes(-rnd.Next(30, 50));
        }
        public void Timer_Tick()
        {
            Current_Time = Current_Time.AddMinutes(1);
            foreach(var courier in Company.couriers)
            {
                if (courier.Taken_Order != null & courier.thread_alive == false)
                {
                    courier.thread.Start();
                }
            }
            Thread.Sleep(Delay);
        }
        public void Get_Time()
        {
            Console.WriteLine($"Текущее время {Convert.ToString(Current_Time.TimeOfDay).Remove(5)}");
        }
    }
}