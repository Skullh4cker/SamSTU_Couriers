using System;
using System.Collections.Generic;

namespace Courier_Upgrade
{
    class Company
    {
        public static decimal Money { get; private set; }
        public static int OrderCounter = 0;
        public const double SpeedMultiplier = 0.01;
        public static int FieldSize = 15;
        public static decimal PricePerKilometer = 1000;

        public const double DefaultFootCourierSpeed = 10 * SpeedMultiplier;
        public const double DefaultScooterCourierSpeed = 30 * SpeedMultiplier;
        public const double DefaultCarCourierSpeed = 60 * SpeedMultiplier;

        public const double DefaultFootCourierCapacity = 20;
        public const double DefaultScooterCourierCapacity = 25;
        public const double DefaultCarCourierCapacity = 100;

        public const double DefaultCourierSalaryMultiplier = 3;

        public static HashSet<Courier> couriers = new HashSet<Courier>();
        public static Queue<Order> new_orders = new Queue<Order>();
        public static List<Order> all_orders = new List<Order>();
        
        public static Dictionary<byte, string> order_statuses = new Dictionary<byte, string>() { { 255, "просрочен..." }, { 0, "не распределён" }, { 1, "распределён" }, { 2, "взят курьером" }, { 3, "доставлен!" } };
        public static List<Position> Dots = new List<Position>();
        public static List<string> courier_names = new List<string>() { "James", "Carl", "Noah", "Oliver", "George", "Arthur", "Leo", "Thomas", "William", "Lucas" };
        
        public static void AddMoney(decimal money)
        {
            Money += money;
        }
        public static void SubtractMoney(decimal order_money)
        {
            Money -= order_money;
        }
        public static bool IsProfitable(Order order, Courier courier)
        {
            if (order.Price > courier.Salary)
                return true;
            else
                return false;
        }
        public static HashSet<Courier> GetRelevantCouriers(Order order)
        {
            HashSet<Courier> couriers = new HashSet<Courier>();
            foreach (var courier in Company.couriers)
            {
                if (courier.CanGet(order))
                {
                     couriers.Add(courier);
                }
            }
            return couriers;
        }
    }
}

