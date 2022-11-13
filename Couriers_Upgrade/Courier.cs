using System;
using System.Threading;

namespace Couriers_Upgrade
{
    abstract class Courier
    {
        public string Name { get; set; }
        protected double Speed { get; set; }
        protected double Capacity { get; set; }
        protected decimal MinPrice { get; set; }
        public bool IsBusy { get; set; }
        public double way_time { get; protected set; }
        public Position CurrentPostion;
        public Order Taken_Order { get; set; }
        public bool thread_alive = false;
        private Random rnd = new Random();
        public Thread thread;
        double x;
        double y;
        public Courier()
        {
            CurrentPostion = new Position(rnd.Next(1, Company.Field_Size), rnd.Next(1, Company.Field_Size), '1');
            thread = new Thread(Test_Movement);
            Company.dots.Add(CurrentPostion);
            Company.couriers.Add(this);
            Thread.Sleep(30);
        }
        // Вывод информации о курьере
        public void Show_Information()
        {
            if(Taken_Order != null)
            {
                Console.WriteLine($"Имя: {Name}; Взятый номер заказа: {Taken_Order.Order_Number}; Скорость: {Speed / Company.SpeedMultiplier}; Грузоподъёмность: {Capacity}; Минимальная цена: {MinPrice}; Позиция: ({CurrentPostion.X}; {CurrentPostion.Y})");
            }
        }
        // Проверяем, может ли курьер взять заказ
        public bool Can_Get(Order order)
        {
            if (this.IsBusy)
            {
                //ColorOutput.Color_Writeline($"{Name} уже взял заказ №{Queue_Order.Order_Number}", ConsoleColor.Red);
                return false;
            }
            else
            {
                double way_to_pickup = Position.GetDistance(CurrentPostion, order.CurrentPostion);
                double all_way_to_deliver = Position.GetDistance(CurrentPostion, order.CurrentPostion) + order.Distance;
                double time_to_deliver = (order.DeliveryTime - Time.Current_Time).TotalMinutes;   //Время, за которое заказ должен быть доставлен
                double time_to_arrive = (order.PickUpTime - Time.Current_Time).TotalMinutes;
                way_time = all_way_to_deliver / Speed;

                if (order.simple_deliver)
                {
                    if (time_to_deliver > (all_way_to_deliver / Speed) & order.Weight < Capacity & order.Price > MinPrice)
                    {
                        //ColorOutput.Color_Writeline($"{Name} готов взять заказ №{order.Order_Number}", ConsoleColor.Yellow);
                        return true;
                    }
                    else
                    {
                        //ColorOutput.Color_Writeline($"{Name} не подходит под заказ №{order.Order_Number}", ConsoleColor.Red);
                        return false;
                    }
                }
                else
                {
                    if (time_to_arrive > (way_to_pickup / Speed) & time_to_deliver > (all_way_to_deliver / Speed) & order.Weight < Capacity & order.Price > MinPrice)
                    {
                        //ColorOutput.Color_Writeline($"{Name} готов взять заказ №{order.Order_Number}", ConsoleColor.Yellow);
                        return true;
                    }
                    else
                    {
                        //ColorOutput.Color_Writeline($"{Name} не подходит под заказ №{order.Order_Number}", ConsoleColor.Red);
                        return false;
                    }
                }
            }
        }
        public void Test_Movement()
        {
            thread_alive = true;
            while (true)
            {
                if (this.Taken_Order != null)
                {

                    switch (this.Taken_Order.Status)
                    {
                        case 1:
                            this.Move_To_PickUp_Tick();
                            break;
                        case 2:
                            this.Move_To_Deliver_Tick();
                            break;
                        case 3:
                            this.thread.Abort();
                            thread_alive = false;
                            break;
                    }
                }
                thread_alive = true;
                Thread.Sleep(500);
            }
        }
        public void Move_To_PickUp_Tick()
        {
            x = CurrentPostion.X;
            y = CurrentPostion.Y;

            if(this.CurrentPostion.X == Taken_Order.CurrentPostion.X & this.CurrentPostion.Y == Taken_Order.CurrentPostion.Y)
            {
                Taken_Order.Status = 2;
            }
            else
            {
                Move(this.CurrentPostion, Taken_Order.CurrentPostion, false);
            }
        }
        public void Move_To_Deliver_Tick()
        {
            if (this.CurrentPostion.X == Taken_Order.Destination.X & this.CurrentPostion.Y == Taken_Order.Destination.Y)
            {
                Taken_Order.Status = 3;
                try
                {
                    Company.new_orders.Dequeue();
                }
                catch
                {

                }
                Company.dots.Remove(Taken_Order.CurrentPostion);
                Company.dots.Remove(Taken_Order.Destination);
                Taken_Order = null;
                IsBusy = false;
            }
            else
            {
                Move(this.CurrentPostion, Taken_Order.Destination, true);
            }
        }
        public void Move(Position courier, Position order, bool order_taken)
        {
            Console.ResetColor();

            double speed_x = 0;
            double speed_y = 0;
            int offset_x = order.X - courier.X;
            int offset_y = order.Y - courier.Y;
            if (offset_x * offset_y != 0)
            {
                speed_x = Speed;
                speed_y = Speed;
                //speed_y = Math.Abs(Speed / ((offset_x / offset_y) + 1));
                //speed_x = Math.Abs((offset_x / offset_y) * speed_y);
            }
            else if (offset_x == 0)
            {
                speed_y = Speed;
                speed_x = 0;
            }
            else if (offset_y == 0)
            {
                speed_x = Speed;
                speed_y = 0;
            }
            if (courier.X == order.X & courier.Y == order.Y)
            {
                speed_x = 0;
                speed_y = 0;
            }
            if (offset_x < 0)
                speed_x = -speed_x;
            if (offset_y < 0)
                speed_y = -speed_y;
            x += speed_x;
            y += speed_y;
            CurrentPostion.X = (int)Math.Round(x);
            CurrentPostion.Y = (int)Math.Round(y);
            if (order_taken)
            {
                Taken_Order.CurrentPostion.X = CurrentPostion.X;
                Taken_Order.CurrentPostion.Y = CurrentPostion.Y;
            }
        }
    }
    class FootCourier : Courier
    {
        //Класс пешего курьера
        decimal DefaultFootCourierMinPrice = 100;
        public FootCourier()
        {
            Speed = Company.DefaultFootCourierSpeed;
            Capacity = Company.DefaultFootCourierCapacity;
            MinPrice = DefaultFootCourierMinPrice;
        }

    }
    class ScooterCourier : Courier
    {
        //Класс курьера на самокате
        decimal DefaultScooterCourierMinPrice = 250;
        public ScooterCourier()
        {
            Speed = Company.DefaultScooterCourierSpeed;
            Capacity = Company.DefaultScooterCourierCapacity;
            MinPrice = DefaultScooterCourierMinPrice;
        }
    }
    class CarCourier : Courier
    {
        //Класс курьера на машине
        decimal DefaultCarCourierMinPrice = 500;
        public CarCourier()
        {
            Speed = Company.DefaultCarCourierSpeed;
            Capacity = Company.DefaultCarCourierCapacity;
            MinPrice = DefaultCarCourierMinPrice;
        }
    }
}