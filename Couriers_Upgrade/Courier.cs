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
        public Order taken_order;
        private Random rnd = new Random();
        public Courier()
        {
            CurrentPostion = new Position(rnd.Next(1, Company.Field_Size), rnd.Next(1, Company.Field_Size));
            Thread.Sleep(30);
        }
        // Вывод информации о курьере
        public void Show_Information()
        {
            Console.WriteLine($"Имя: {Name}; Скорость: {Speed/Company.SpeedMultiplier}; Грузоподъёмность: {Capacity}; Минимальная цена: {MinPrice}; Позиция: ({CurrentPostion.X}; {CurrentPostion.Y})");
        }
        // Проверяем, может ли курьер взять заказ
        public bool Can_Get(Deliver deliver)
        {
            if (this.IsBusy)
            {
                ColorOutput.Color_Writeline($"{this.Name} уже взял заказ №{this.taken_order.Order_Number}", ConsoleColor.Red);
                return false;
            }
            else
            {
                double time_to_deliver = (deliver.OrderTime - DateTime.Now).TotalMinutes;   //Время, за которое заказ должен быть доставлен
                double all_way_to_deliver = Position.GetDistance(this.CurrentPostion, deliver.CurrentPostion) + deliver.Distance;   //Полный путь курьера  
                way_time = all_way_to_deliver / this.Speed; //Время, за которое курьер справится с работой
                //Информация для отладки:
                //Console.WriteLine("{0}; Путь для доставки: {1:#.##}; Общее время на доставку: {2:#.##}; Время всего пути: {3:#.##}", this.Name, all_way_to_deliver, time_to_deliver, way_time);

                if (time_to_deliver > (all_way_to_deliver / this.Speed) & deliver.Weight < this.Capacity & deliver.Price > this.MinPrice)
                {
                    ColorOutput.Color_Writeline($"{this.Name} готов взять заказ №{deliver.Order_Number}", ConsoleColor.Yellow);
                    return true;
                }
                else
                {
                    ColorOutput.Color_Writeline($"{this.Name} не подходит под заказ №{deliver.Order_Number}", ConsoleColor.Red);
                    return false;
                }
            }
        }
        public bool Can_Get(PickUpOrder pickuporder)
        {
            if (this.IsBusy)
            {
                ColorOutput.Color_Writeline($"{this.Name} уже взял заказ №{this.taken_order.Order_Number}", ConsoleColor.Red);
                return false;
            }
            else
            {
                double time_to_deliver = (pickuporder.OrderTime2 - DateTime.Now).TotalMinutes;  //Время, за которое заказ должен быть доставлен
                double time_to_arrive = (pickuporder.OrderTime - DateTime.Now).TotalMinutes;    //Время, за которое заказ должен быть взят
                double all_way_to_deliver = Position.GetDistance(this.CurrentPostion, pickuporder.CurrentPostion) + pickuporder.Distance;   //Полный путь курьера
                double way_to_pickup = Position.GetDistance(this.CurrentPostion, pickuporder.CurrentPostion);   //Путь курьера до взятия заказа
                way_time = all_way_to_deliver / this.Speed; //Время, за которое курьер справится с работой
                //Информация для отладки:
                //Console.WriteLine("{0}; Путь для доставки: {1:#.##}; Общее время на доставку: {2:#.##}; Время на прибытие: {3:#.##}; Время всего пути: {4:#.##}", this.Name, all_way_to_deliver, time_to_deliver, time_to_arrive, way_time);

                if (time_to_arrive > (way_to_pickup / this.Speed) & time_to_deliver > (all_way_to_deliver / this.Speed) & pickuporder.Weight < this.Capacity & pickuporder.Price > this.MinPrice)
                {
                    ColorOutput.Color_Writeline($"{this.Name} готов взять заказ №{pickuporder.Order_Number}", ConsoleColor.Yellow);
                    return true;
                }
                else
                {
                    ColorOutput.Color_Writeline($"{this.Name} не подходит под заказ №{pickuporder.Order_Number}", ConsoleColor.Red);
                    return false;
                }
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
        decimal DefaultScooterCourierMinPrice = 250
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
