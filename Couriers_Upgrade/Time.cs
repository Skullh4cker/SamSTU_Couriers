using System;
using System.Threading;

namespace Courier_Upgrade
{
class Time
    {
        public static DateTime Current_Time { get; private set; }
        public static int Delay = 30;
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
        public static void TimerTick()
        {
            Current_Time = Current_Time.AddMinutes(1);
            foreach(var courier in Company.couriers)
            {
                if (courier.TakenOrder != null)
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
        public static double GetDifference(DateTime date1, DateTime date2)
        {
            return (date2 - date1).TotalMinutes;
        }
        public static void PrintTime()
        {
            Console.WriteLine("Текущее время {0}", Convert.ToString(Current_Time.TimeOfDay).Remove(5));
        }
        public static DateTime GetTime()
        {
            return Current_Time;
        }
    }
}