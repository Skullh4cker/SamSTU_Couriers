using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace Courier_Upgrade
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
        public Order TakenOrder { get; set; }
        public List<Order> PlanningOrders = new List<Order>();

        private Random rnd = new Random();
        public List<double> time_m = new List<double>();
        double x;
        double y;
        public Courier()
        {
            Thread.Sleep(1);
            CurrentPosition = new Position(rnd.Next(1, Company.FieldSize), rnd.Next(1, Company.FieldSize), 'C');
            x = CurrentPosition.X;
            y = CurrentPosition.Y;
            Company.Dots.Add(CurrentPosition);
            Company.couriers.Add(this);
            Name = Company.courier_names[rnd.Next(1, Company.courier_names.Count)];
        }
        public void ShowInfromation()
        {
            if (TakenOrder != null)
            {
                Console.WriteLine("Имя: {0}; Взятый номер заказа: {1}; Скорость: {2}; Грузоподъёмность: {3}; Минимальная цена: {4}; Позиция: ({5}; {6})", Name, TakenOrder.OrderNumber, Speed / Company.SpeedMultiplier, Capacity, Salary, CurrentPosition.X, CurrentPosition.Y);
            }
            else
            {
                Console.WriteLine("Имя: {0}; Скорость: {1}; Грузоподъёмность: {2}; Минимальная цена: {3}; Позиция: ({4}; {5})", Name, Speed / Company.SpeedMultiplier, Capacity, Salary, CurrentPosition.X, CurrentPosition.Y);
            }
        }
        public bool CanGet(Order order4)
        {
            if (this.IsBusy)
            {
                if (PlanningOrders.Count > 0)
                {
                    DateTime release = new DateTime();
                    double free_time = 0;
                    for (int i = 0; i < time_m.Count; i++)
                    {
                        free_time += time_m[i];
                    }
                    release = Time.Current_Time.AddMinutes(free_time);
                    return CheckParameters(order4, release, PlanningOrders.Last().Destination);
                }
                else
                {
                    double free_time = 0;
                    DateTime release = new DateTime();
                    free_time += TakenOrder.Plan.EstimatedTimeOfExecution;
                    release = Time.Current_Time.AddMinutes(free_time);
                    return CheckParameters(order4, release, TakenOrder.Destination);
                }
            }
            else
            {
                return CheckParameters(order4, Time.Current_Time, CurrentPosition);
            }
        }
        public bool CheckParameters(Order order2, DateTime time, Position courier_position)
        {
            double way_to_pickup = Position.GetDistance(courier_position, order2.CurrentPostion);
            double all_way_to_deliver = Position.GetDistance(courier_position, order2.CurrentPostion) + order2.Distance;
            double time_to_deliver = (order2.DeliveryTime - time).TotalMinutes;
            double time_to_arrive = (order2.PickUpTime - time).TotalMinutes;
            way_time = FindWayTime(order2, courier_position);

            if (!(order2.Price > Salary & order2.Weight < Capacity))
                return false;
            else
            {
                if (order2.SimpleDeliever)
                    return time_to_deliver > (all_way_to_deliver / Speed);
                else
                    return (time_to_arrive > (way_to_pickup / Speed) & time_to_deliver > (all_way_to_deliver / Speed));
            }
        }
        public double FindWayTime(Order order1, Position courier_position)
        {
            double all_way_to_deliver = Position.GetDistance(courier_position, order1.CurrentPostion) + order1.Distance;
            return all_way_to_deliver / Speed;
        }
        public double FindWayTime2(Order order1)
        {
            double all_way_to_deliver = Position.GetDistance(CurrentPosition, order1.CurrentPostion) + order1.Distance;
            return all_way_to_deliver / Speed;
        }
        public PlanningOption RequestPlanning(Order order)
        {
            var planningOption = new PlanningOption();
            planningOption.Courier = this;
            planningOption.Order = order;
            planningOption.CourierPrice = (decimal)(Position.GetDistance(this.CurrentPosition, order.CurrentPostion) * Company.DefaultCourierSalaryMultiplier);
            if(PlanningOrders.Count > 0)
            {
                planningOption.EstimatedTimeOfExecution = FindWayTime(order, PlanningOrders.Last().Destination);
            }
            else
            {
                planningOption.EstimatedTimeOfExecution = FindWayTime2(order);
            }
            
            return planningOption;
        }
        public void OrderMovement()
        {
            while (true)
            {
                if (this.TakenOrder != null)
                {

                    switch (this.TakenOrder.Status)
                    {
                        case 1:
                            this.MoveToPickUpTick();
                            break;
                        case 2:
                            this.MoveToDelieverTick();
                            break;
                        case 3:
                            break;
                    }
                }
                break;
            }
        }
        public void RandomMovement()
        {
            Thread.Sleep(1);
            Move(this.CurrentPosition, new Position(rnd.Next(1, Company.FieldSize), rnd.Next(1, Company.FieldSize), '.'), false);
        }
        public void MoveToPickUpTick()
        {

            if(this.CurrentPosition.X == TakenOrder.CurrentPostion.X & this.CurrentPosition.Y == TakenOrder.CurrentPostion.Y)
            {
                TakenOrder.Status = 2;
            }
            else
            {
                Move(this.CurrentPosition, TakenOrder.CurrentPostion, false);
            }
        }
        public void MoveToDelieverTick()
        {
            if (this.CurrentPosition.X == TakenOrder.Destination.X & this.CurrentPosition.Y == TakenOrder.Destination.Y)
            {
                TakenOrder.Status = 3;
                try
                {
                    TakenOrder.DeliverOrder(this);
                }
                catch
                {

                }
                time_m.Remove(TakenOrder.Plan.EstimatedTimeOfExecution);
                TakenOrder = null;
                IsBusy = false;
                if (PlanningOrders.Count > 0)
                    ChooseOrder();
            }
            else
            {
                Move(this.CurrentPosition, TakenOrder.Destination, true);
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
                TakenOrder.CurrentPostion.X = CurrentPosition.X;
                TakenOrder.CurrentPostion.Y = CurrentPosition.Y;
            }
        }
        public void AddOrderToPlan(Order order)
        {
            time_m.Add(order.Plan.EstimatedTimeOfExecution);
            PlanningOrders.Add(order);
            if (!IsBusy)
                ChooseOrder();
        }
        public void ChooseOrder()
        {
            PlanningOrders = PlanningOrders.OrderBy(plan => Position.GetDistance(plan.CurrentPostion, CurrentPosition)).ToList();
            if(PlanningOrders.Count > 0)
            {
                var order = PlanningOrders[0];
                PlanningOrders.RemoveAt(0);
                TakenOrder = order;
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