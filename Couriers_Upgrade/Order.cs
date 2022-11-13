using System;
using System.Threading;
using System.Collections.Generic;

namespace Couriers_Upgrade
{
    abstract class Order
    {
        public bool simple_deliver { get; protected set; }
        public int Order_Number { get; set; }
        public Position CurrentPostion;
        public Position Destination;
        public DateTime DeliveryTime;
        public DateTime PickUpTime;
        public double Distance { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; protected set; }
        public byte Status { get; set; }
        Random rnd = new Random();
        public Order()
        {
            Status = 0;
            Weight = rnd.Next(1, 50);
            CurrentPostion = new Position(rnd.Next(1, Company.Field_Size), rnd.Next(1, Company.Field_Size), '*');
            Destination = CurrentPostion;
            if (Destination == CurrentPostion)
                Destination = new Position(rnd.Next(1, Company.Field_Size), rnd.Next(1, Company.Field_Size), '^');
            DeliveryTime = Time.Add_Random_Time(Time.Current_Time);
            Distance = Get_Order_Distance();
            Price = Get_Order_Price(Distance);
            Company.new_orders.Enqueue(this);
            Order_Number = Company.new_orders.Count;
            Company.dots.Add(CurrentPostion);
            Company.dots.Add(Destination);
            Thread.Sleep(30);
        }
        /// <summary>
        /// Показать информацию о заказе
        /// </summary>
        public void Show_Information()
        {
            Console.Write("Номер: {0}; Статус: ", Order_Number);
            switch (Status)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.Write("{0}", Company.order_statuses[Status]);
            Console.ResetColor();
            Console.WriteLine("; Вес: {0}; Цена: {1:#.##}; Текущая позиция: ({2}; {3}); Место назначения: ({4}; {5}); Доставить к {6}", Weight, Price, CurrentPostion.X, CurrentPostion.Y, Destination.X, Destination.Y, Convert.ToString(DeliveryTime.TimeOfDay).Remove(5));

        }
        /// <summary>
        /// Получить дистанцию, на которую надо доставить заказ
        /// </summary>
        /// <returns></returns>
        private double Get_Order_Distance()
        {
            return Position.GetDistance(CurrentPostion, Destination);
        }
        /// <summary>
        /// Получить цену заказа
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private decimal Get_Order_Price(double distance)
        {
            return (decimal)distance * Company.PricePerKiloMeter;
        }
        public void Choose_Best()
        {
            double comparator = double.MaxValue;
            HashSet<Courier> relevant_couriers = new HashSet<Courier>();
            foreach (var courier in Company.couriers)
            {
                if (courier.Can_Get(this) & courier.IsBusy != true)
                {
                    relevant_couriers.Add(courier);
                }
            }
            foreach (var courier in relevant_couriers)
            {
                if (courier.way_time < comparator)
                {
                    comparator = courier.way_time;
                }
            }
            foreach (var courier in relevant_couriers)
            {
                if (courier.way_time == comparator)
                {
                    //ColorOutput.Color_Writeline($"Заказ №{Order_Number} выдан курьеру {courier.Name} как самому подходящему", ConsoleColor.Green);
                    Status = 1;
                    courier.Taken_Order = this;
                    courier.IsBusy = true;
                }
            }
            relevant_couriers.Clear();
        }

    }
    // Класс доставки
    class Deliver : Order
    {
        public Deliver()
        {
            simple_deliver = true;
        }
    }
    // Класс заказа с определённым временем взятия
    class PickUpOrder : Order
    {
        public PickUpOrder()
        {
            simple_deliver = false;
            PickUpTime = Time.Subtract_Random_Time(DeliveryTime);
        }
        new public void Show_Information()
        {
            Console.Write("Номер: {0}; Статус: ", Order_Number);
            switch (Status)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            Console.Write("{0}", Company.order_statuses[Status]);
            Console.ResetColor();
            Console.WriteLine("; Вес: {0}; Цена: {1:#.##}; Текущая позиция: ({2}; {3}); Место назначения: ({4}; {5}); Забрать в: {6}; Доставить к {7}", Weight, Price, CurrentPostion.X, CurrentPostion.Y, Destination.X, Destination.Y, Convert.ToString(PickUpTime.TimeOfDay).Remove(5), Convert.ToString(DeliveryTime.TimeOfDay).Remove(5));
        }
    }
}