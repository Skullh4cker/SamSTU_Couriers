using System;
using System.Threading;

namespace SamSTU_Couriers
{
    abstract class Courier
    {
        public string Name { get; set; }
        protected double Speed { get; set; }
        protected double Capacity { get; set; }
        public decimal Salary { get; protected set; }
        public bool IsBusy { get; set; }
        public double way_time { get; protected set; }
        public Position CurrentPosition;
        public Order Taken_Order { get; set; }
        public bool thread_alive = false;
        private Random rnd = new Random();
        //public Thread thread;
        double x;
        double y;
        public Courier()
        {
            Thread.Sleep(1);
            CurrentPosition = new Position(rnd.Next(1, Company.FieldSize), rnd.Next(1, Company.FieldSize), 'C');
            x = CurrentPosition.X;
            y = CurrentPosition.Y;
            //thread = new Thread(ThreadMovement);
            Company.Dots.Add(CurrentPosition);
            Company.couriers.Add(this);
            Name = Company.courier_names[rnd.Next(1, Company.courier_names.Count)];
        }
        // Вывод информации о курьере
        public void ShowInfromation()
        {
            if (Taken_Order != null)
            {
                Console.WriteLine("Имя: {0}; Взятый номер заказа: {1}; Скорость: {2}; Грузоподъёмность: {3}; Минимальная цена: {4}; Позиция: ({5}; {6})", Name, Taken_Order.OrderNumber, Speed / Company.SpeedMultiplier, Capacity, Salary, CurrentPosition.X, CurrentPosition.Y);
            }
            else
            {
                Console.WriteLine("Имя: {0}; Скорость: {1}; Грузоподъёмность: {2}; Минимальная цена: {3}; Позиция: ({4}; {5})", Name, Speed / Company.SpeedMultiplier, Capacity, Salary, CurrentPosition.X, CurrentPosition.Y);
            }
        }
        // Проверяем, может ли курьер взять заказ
        public bool CanGet(Order order)
        {
            if (this.IsBusy)
            {
                //ColorOutput.Color_Writeline($"{Name} уже взял заказ №{Queue_Order.Order_Number}", ConsoleColor.Red);
                return false;
            }
            else
            {
                double way_to_pickup = Position.GetDistance(CurrentPosition, order.CurrentPostion);
                double all_way_to_deliver = Position.GetDistance(CurrentPosition, order.CurrentPostion) + order.Distance;
                double time_to_deliver = (order.DeliveryTime - Time.Current_Time).TotalMinutes;   //Время, за которое заказ должен быть доставлен
                double time_to_arrive = (order.PickUpTime - Time.Current_Time).TotalMinutes;
                way_time = all_way_to_deliver / Speed;

                if (order.SimpleDeliever)
                {
                    if (time_to_deliver > (all_way_to_deliver / Speed) & order.Weight < Capacity & order.Price > Salary)
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
                    if (time_to_arrive > (way_to_pickup / Speed) & time_to_deliver > (all_way_to_deliver / Speed) & order.Weight < Capacity & order.Price > Salary)
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
        public void OrderMovement()
        {
            //thread_alive = true;
            while (true)
            {
                if (this.Taken_Order != null)
                {

                    switch (this.Taken_Order.Status)
                    {
                        case 1:
                            this.MoveToPickUpTick();
                            break;
                        case 2:
                            this.MoveToDelieverTick();
                            break;
                        case 3:
                            //thread_alive = false;
                            break;
                    }
                }
                //thread_alive = true;
                break;
                //Thread.Sleep(Time.Delay);
            }
        }
        public void RandomMovement()
        {
            Thread.Sleep(1);
            Move(this.CurrentPosition, new Position(rnd.Next(1, Company.FieldSize), rnd.Next(1, Company.FieldSize), '.'), false);
        }
        public void MoveToPickUpTick()
        {

            if(this.CurrentPosition.X == Taken_Order.CurrentPostion.X & this.CurrentPosition.Y == Taken_Order.CurrentPostion.Y)
            {
                Taken_Order.Status = 2;
            }
            else
            {
                Move(this.CurrentPosition, Taken_Order.CurrentPostion, false);
            }
        }
        public void MoveToDelieverTick()
        {
            if (this.CurrentPosition.X == Taken_Order.Destination.X & this.CurrentPosition.Y == Taken_Order.Destination.Y)
            {
                Taken_Order.Status = 3;
                try
                {
                    Taken_Order.DeliverOrder(this);
                }
                catch
                {

                }
                Taken_Order = null;
                IsBusy = false;
            }
            else
            {
                Move(this.CurrentPosition, Taken_Order.Destination, true);
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
                speed_x = Speed / Math.Sqrt(2);
                speed_y = Speed / Math.Sqrt(2);
            }
            else if (offset_x == 0)
            {
                speed_y = this.Speed;
                speed_x = 0;
            }
            else if (offset_y == 0)
            {
                speed_x = this.Speed;
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

            if ((int)Math.Round(x) > CurrentPosition.X || (int)Math.Round(x) < CurrentPosition.X) 
            {
                CurrentPosition.X = (int)Math.Round(x);
                x = CurrentPosition.X;
            }
            if ((int)Math.Round(y) > CurrentPosition.Y || (int)Math.Round(y) < CurrentPosition.Y)
            {
                CurrentPosition.Y = (int)Math.Round(y);
                y = CurrentPosition.Y;
            }
            if (order_taken)
            {
                Taken_Order.CurrentPostion.X = CurrentPosition.X;
                Taken_Order.CurrentPostion.Y = CurrentPosition.Y;
            }
        }
        public void TakeOrder(Order order)
        {
            if (Company.IsProfitable(order, this))
            {
                Taken_Order = order;
                IsBusy = true;
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
            Salary = DefaultFootCourierMinPrice;
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
            Salary = DefaultScooterCourierMinPrice;
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
            Salary = DefaultCarCourierMinPrice;
        }
    }
}