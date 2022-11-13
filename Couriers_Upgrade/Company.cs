using System;
using System.Collections.Generic;

namespace Couriers_Upgrade
{
    class Company
    {
        public const double SpeedMultiplier = 0.01;
        public const int Field_Size = 15;
        public const decimal PricePerKiloMeter = 1000;

        public const double DefaultFootCourierSpeed = 60 * SpeedMultiplier;
        public const double DefaultScooterCourierSpeed = 60 * SpeedMultiplier;
        public const double DefaultCarCourierSpeed = 60 * SpeedMultiplier;

        public const double DefaultFootCourierCapacity = 50;
        public const double DefaultScooterCourierCapacity = 50;
        public const double DefaultCarCourierCapacity = 100;

        public static HashSet<Courier> couriers = new HashSet<Courier>();
        public static Queue<Order> new_orders = new Queue<Order>();
        public static Dictionary<byte, string> order_statuses = new Dictionary<byte, string>() { { 0, "не распределён" }, { 1, "распределён" }, { 2, "взят курьером" }, { 3, "доставлен!" } };
        public static List<Position> dots = new List<Position>();
    }
}

