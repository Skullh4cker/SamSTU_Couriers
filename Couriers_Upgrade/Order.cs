using System;
using System.Threading;

namespace Couriers_Upgrade
{
    abstract class Order
    {
        public int Order_Number { get; set; }
        public Position CurrentPostion;
        public Position Destination;
        public DateTime OrderTime = DateTime.Now;
        public DateTime OrderTime2 = DateTime.Now;
        public double Distance { get; set; }
        public double Weight { get; set; }
        public decimal Price { get; protected set; }
        Random rnd = new Random();
        public Order()
        {
            Weight = rnd.Next(1, 50);
            CurrentPostion = new Position(rnd.Next(1, Company.Field_Size), rnd.Next(1, Company.Field_Size));
            Destination = CurrentPostion;
            if(Destination == CurrentPostion)
                Destination = new Position(rnd.Next(1, Company.Field_Size), rnd.Next(1, Company.Field_Size));
            OrderTime = Time.Random_Time(OrderTime);
            Get_Order_Distance();
            Price = (decimal)Distance * Company.PricePerKiloMeter;
            Thread.Sleep(30);
        }
        // Вывод информации о заказе
        public void Show_Information()
        {
            Console.WriteLine("Номер: {0:#.##}; Вес: {1:#.##}; Цена: {2:#.##}; Текущая позиция: ({3}; {4}); Место назначения: ({5}; {6}); Доставить к {7}", Order_Number, Weight, Price, CurrentPostion.X, CurrentPostion.Y, Destination.X, Destination.Y, Convert.ToString(OrderTime.TimeOfDay).Remove(5));
        }
        // Получаем расстояние от текущей позиции заказа к точке доставки
        public void Get_Order_Distance()
        {
            Distance = Position.GetDistance(CurrentPostion, Destination);
        }
    }
    // Класс доставки
    class Deliver : Order
    {
        public Deliver()
        {

        }
    }
    // Класс заказа с определённым временем взятия
    class PickUpOrder : Order
    {
        public PickUpOrder()
        {
            OrderTime2 = Time.Random_Time(OrderTime);
        }
        new public void Show_Information()
        {
            Console.WriteLine("Номер: {0:#.##}; Вес: {1:#.##}; Цена: {2:#.##}; Текущая позиция: ({3}; {4}); Место назначения: ({5}; {6}); Забрать в {7}; Доставить к {8}", Order_Number, Weight, Price, CurrentPostion.X, CurrentPostion.Y, Destination.X, Destination.Y, Convert.ToString(OrderTime.TimeOfDay).Remove(5), Convert.ToString(OrderTime2.TimeOfDay).Remove(5));
        }
    }
}
