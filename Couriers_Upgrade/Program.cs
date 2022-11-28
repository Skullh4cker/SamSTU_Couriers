using System;
using System.Collections.Generic;
using System.Threading;

namespace SamSTU_Couriers
{
    class Program
    {
        static void Main(string[] args)
        {
            int courier_count = 5;
            int order_count = 20;
            //Console.Write("Введите размер поля: ");
            //int.TryParse(Console.ReadLine(), out Company.FieldSize);
            Company.PricePerKilometer = Company.FieldSize / (decimal)0.0214;
            Random rnd = new Random();

            Time time = new Time();
            char[,] pixels = new char[Company.FieldSize, Company.FieldSize];
            bool exit = false;
            Console.Write("Введите число курьеров: ");
            int.TryParse(Console.ReadLine(), out courier_count);
            Console.Write("Введите число заказов для выполнения: ");
            int.TryParse(Console.ReadLine(), out order_count);
            Console.Clear();
            for(int i = 0; i < courier_count; i++)
            {
                ScooterCourier scooter = new ScooterCourier();
            }
            while (true)
            {
                if(Company.OrderCounter == order_count)
                {
                    foreach (var order in Company.all_orders)
                    {
                        if (order.Status == 3 || order.Status == 255)
                        {
                            exit = true;
                        }
                        else
                        {
                            exit = false;
                            break;
                        }
                    }
                }

                Console.WriteLine("Деньги компании: {0:#.##}$", Company.Money);
                Time.PrintTime();

                while (Company.OrderCounter < order_count)
                {
                    Deliver delivery = new Deliver();
                    delivery.DeliveryTime = DateTime.Now.AddHours(5);
                }

                for (int i = 0; i < Company.FieldSize; i++)
                {
                    for (int j = 0; j < Company.FieldSize; j++)
                    {
                        pixels[i, j] = '.';
                    }
                }

                foreach (var order in Company.new_orders)
                {
                    if (order.Status == 0)
                    {
                        order.ChooseBest();
                    }
                }
                foreach (var order in Company.all_orders)
                {
                    order.CheckOverdue();
                }
                
                PrintInformation();
                
                foreach (var dot in Company.Dots)
                {
                    if (pixels[dot.Y - 1, dot.X - 1] == 'C')
                    {
                        continue;
                    }
                    if (dot.CoordSymbol == 'C')
                    {
                        pixels[dot.Y - 1, dot.X - 1] = dot.CoordSymbol;
                    }
                    else if (dot.CoordSymbol == '*' || dot.CoordSymbol == '^')
                    {
                        pixels[dot.Y - 1, dot.X - 1] = dot.CoordSymbol;
                    }
                }
                
                for (int i = 0; i < Company.FieldSize; i++)
                {
                    for (int j = 0; j < Company.FieldSize; j++)
                    {
                        Console.Write("{0} ", pixels[i, j]);
                    }
                    Console.WriteLine();
                }
                
                Time.TimerTick();
                //Console.Clear();
                Console.SetCursorPosition(0, 0);
                if (exit)
                    break;
            }
            //Console.SetCursorPosition(0, 6 + Company.FieldSize + Company.all_orders.Count + Company.couriers.Count);
            //ColorOutput.ColorWriteLine("Работа завершена!", ConsoleColor.Green);
            Console.ReadKey();
        }
        static void PrintInformation()
        {
            
            foreach (var courier in Company.couriers)
            {
                courier.ShowInfromation();
            }
            
            
            foreach (var order in Company.all_orders)
            {
                order.ShowInformation();
            }
            
            Console.WriteLine("Схема: курьеры(C), точки взятия заказов(*), точки доставки(^)");
        }
    }
}
