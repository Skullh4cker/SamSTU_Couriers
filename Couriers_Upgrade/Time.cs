using System;
using System.Threading;

namespace Couriers_Upgrade
{
    class Time
    {
        // Функция, которая добавляет к полученному моменту времени от 10 до 80 минут, тем самым генерируя случайное время заказа
        public static DateTime Random_Time(DateTime date)
        {
            Random rnd = new Random();
            Thread.Sleep(30);
            return date.AddMinutes(rnd.Next(10, 80));
        }
    }
}
