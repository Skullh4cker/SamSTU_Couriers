using System;

namespace Couriers_Upgrade
{
    class Company
    {
        public const double SpeedMultiplier = 0.1;
        public const int Field_Size = 15;
        public const decimal PricePerKiloMeter = 100;

        public const double DefaultFootCourierSpeed = 7 * SpeedMultiplier;
        public const double DefaultScooterCourierSpeed = 30 * SpeedMultiplier;
        public const double DefaultCarCourierSpeed = 60 * SpeedMultiplier;

        public const double DefaultFootCourierCapacity = 25;
        public const double DefaultScooterCourierCapacity = 40;
        public const double DefaultCarCourierCapacity = 100;

        public const decimal DefaultFootCourierMinPrice = 100;
        public const decimal DefaultScooterCourierMinPrice = 250;
        public const decimal DefaultCarCourierMinPrice = 500;
    }
}
