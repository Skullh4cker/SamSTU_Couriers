using System;
using System.Collections.Generic;
using System.Threading;

namespace Couriers_Upgrade
{
    class Program
    {
        static void Main(string[] args)
        {
            Time time = new Time();
           
            var on_foot = new FootCourier() { Name = "James" };
            //var on_car = new CarCourier() { Name = "Ryan" };
            var on_scooter = new ScooterCourier() { Name = "Bob" };

            var order1 = new Deliver();
            var order2 = new Deliver();
            var order3 = new Deliver();

            while (true)
            {
                time.Get_Time();
                char[,] pixels = new char[Company.Field_Size, Company.Field_Size];
                for (int i = 0; i < Company.Field_Size; i++)
                {
                    for (int j = 0; j < Company.Field_Size; j++)
                    {
                        pixels[i, j] = '.';
                    }
                }
                foreach (var order in Company.new_orders)
                {
                    if(order.Status == 0)
                    {
                        order.Choose_Best();
                    }
                }

                on_foot.Show_Information();
                on_scooter.Show_Information();
                //on_car.Show_Information();

                order1.Show_Information();
                order2.Show_Information();
                order3.Show_Information();

                Console.WriteLine("Схема: курьеры(1), точки взятия заказов(*), точки доставки(^)");
                foreach (var dot in Company.dots)
                {
                    if(pixels[dot.Y - 1, dot.X - 1] == 'C')
                    {
                        continue;
                    }
                    if(dot.coord_symbol == 'C')
                    {
                        pixels[dot.Y-1, dot.X-1] = dot.coord_symbol;
                    }
                    else if(dot.coord_symbol == '*' || dot.coord_symbol == '^')
                    {
                        pixels[dot.Y-1, dot.X-1] = dot.coord_symbol;
                    }
                }
                
                for (int i = 0; i < Company.Field_Size; i++)
                {
                    for (int j = 0; j < Company.Field_Size; j++)
                    {
                        Console.Write($"{pixels[i, j]} ");
                    }
                    Console.WriteLine("");
                }
                
                time.Timer_Tick();
                //Console.SetCursorPosition(0, 0);
                Console.Clear();
            }
        }
    }
}
