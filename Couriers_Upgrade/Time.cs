using System;
using System.Threading;

namespace SamSTU_Couriers
{
    class Time
    {
        public static DateTime Current_Time { get; private set; }
        public static int Delay = 50;
        public Time()
        {
            Current_Time = DateTime.Now;
        }
        public static DateTime AddRandomTime(DateTime date)
        {
            Random rnd = new Random();
            Thread.Sleep(1);
            return date.AddMinutes(rnd.Next(50, 60));
        }
        public static DateTime SubtractRandomTime(DateTime date)
        {
            Random rnd = new Random();
            Thread.Sleep(1);
            return date.AddMinutes(-rnd.Next(30, 40));
        }
        public void TimerTick()
        {
            Current_Time = Current_Time.AddMinutes(1);
            foreach(var courier in Company.couriers)
            {
                if (courier.Taken_Order != null)
                {
                    courier.OrderMovement();
                }
                /*
                else
                {
                    courier.RandomMovement();
                }*/
            }
            Thread.Sleep(Delay);
        }
        public void PrintTime()
        {
            Console.WriteLine("Текущее время {0}", Convert.ToString(Current_Time.TimeOfDay).Remove(5));
        }
        public static DateTime GetTime()
        {
            return Current_Time;
        }
    }
}