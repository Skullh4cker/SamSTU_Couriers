using System;
using System.Threading;
using System.Collections.Generic;

namespace SamSTU_Couriers
{
    abstract class Order
    {
        public bool SimpleDeliever { get; protected set; }
        public int OrderNumber { get; set; }
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
            Thread.Sleep(1);
            Status = 0;
            Weight = rnd.Next(1, 5);
            CurrentPostion = new Position(rnd.Next(1, Company.FieldSize), rnd.Next(1, Company.FieldSize), '*');
            Destination = CurrentPostion;
            if (Destination == CurrentPostion)
                Destination = new Position(rnd.Next(1, Company.FieldSize), rnd.Next(1, Company.FieldSize), '^');
            DeliveryTime = Time.AddRandomTime(Time.Current_Time);
            Distance = GetOrderDistance();
            Price = GetOrderPrice(Distance);
            Company.new_orders.Enqueue(this);
            Company.all_orders.Add(this);

            OrderNumber = Company.OrderCounter;
            Company.OrderCounter++;

            Company.Dots.Add(CurrentPostion);
            Company.Dots.Add(Destination);
        }

        /// <summary>
        /// Показать информацию о заказе
        /// </summary>
        public void ShowInformation()
        {
            Console.Write("Номер: {0}; Статус: ", OrderNumber);
            switch (Status)
            {
                case 0:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case 255:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
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
        private double GetOrderDistance()
        {
            return Position.GetDistance(CurrentPostion, Destination);
        }
        /// <summary>
        /// Получить цену заказа
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private decimal GetOrderPrice(double distance)
        {
            return (decimal)distance * Company.PricePerKilometer;
        }
        /// <summary>
        /// Выбрать лучшего курьера
        /// </summary>
        public void ChooseBest()
        {
            double comparator = double.MaxValue;
            HashSet<Courier> relevant_couriers = new HashSet<Courier>();
            foreach (var courier in Company.couriers)
            {
                if (courier.CanGet(this) & !courier.IsBusy)
                {
                    relevant_couriers.Add(courier);
                }
            }

            //var best = relevant_couriers.OrderBy(x => x.way_time).FirstOrDefault();

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
                    courier.TakeOrder(this);
                }
            }
            relevant_couriers.Clear();
        }
        /// <summary>
        /// Доставить заказ
        /// </summary>
        /// <param name="courier"></param>
        public void DeliverOrder(Courier courier)
        {
            Company.new_orders.Dequeue();
            Company.Dots.Remove(CurrentPostion);
            Company.Dots.Remove(Destination);
            Company.AddMoney(Price, courier.Salary);
        }
        /// <summary>
        /// Проверить, является ли данный заказ просроченным
        /// </summary>
        public void CheckOverdue()
        {
            if (Status == 0 && DeliveryTime < Time.GetTime())
            {
                Status = 255;
                Company.new_orders.Dequeue();
                Company.Dots.Remove(CurrentPostion);
                Company.Dots.Remove(Destination);
                Company.SubtractMoney(this.Price);
                return;
            }
        }
    }
    class Deliver : Order
    {
        public Deliver()
        {
            SimpleDeliever = true;
        }
    }
    class PickUpOrder : Order
    {
        public PickUpOrder()
        {
            SimpleDeliever = false;
            PickUpTime = Time.SubtractRandomTime(DeliveryTime);
        }
        new public void Show_Information()
        {
            Console.Write("Номер: {0}; Статус: ", OrderNumber);
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