using System;
using System.Collections.Generic;
using System.Threading;

namespace Couriers_Upgrade
{
    class Program
    {
        static void Main(string[] args)
        {
            for(; ; )
            {
                Time.Time_Now();
                HashSet<Courier> couriers = new HashSet<Courier>();
                HashSet<Courier> relevant_courier = new HashSet<Courier>();
                HashSet<Deliver> deliver_orders = new HashSet<Deliver>();
                HashSet<PickUpOrder> pickup_orders = new HashSet<PickUpOrder>();
                double comparator = double.MaxValue;

                var on_foot = new FootCourier() { Name = "James" };
                var on_scooter = new ScooterCourier() { Name = "Bob" };
                var on_car = new CarCourier() { Name = "Ryan" };

                on_foot.Show_Information();
                on_scooter.Show_Information();
                on_car.Show_Information();

                couriers.Add(on_foot);
                couriers.Add(on_scooter);
                couriers.Add(on_car);

                var order1 = new Deliver() { Order_Number = 1 };
                var order2 = new Deliver() { Order_Number = 2 };
                var order3 = new PickUpOrder() { Order_Number = 3 };

                order1.Show_Information();
                order2.Show_Information();
                order3.Show_Information();

                deliver_orders.Add(order1);
                deliver_orders.Add(order2);
                pickup_orders.Add(order3);

                foreach (var deliver_order in deliver_orders)
                {
                    foreach (var courier in couriers)
                    {
                        if (courier.Can_Get(deliver_order) & courier.IsBusy != true)
                        {
                            relevant_courier.Add(courier);
                        }
                    }
                    foreach (var courier in relevant_courier)
                    {
                        if (courier.way_time < comparator)
                        {
                            comparator = courier.way_time;
                        }
                    }
                    foreach (var courier in relevant_courier)
                    {
                        if (courier.way_time == comparator)
                        {
                            ColorOutput.Color_Writeline($"Заказ №{deliver_order.Order_Number} выдан курьеру {courier.Name} как самому подходящему", ConsoleColor.Green);
                            courier.taken_order = deliver_order;
                            courier.IsBusy = true;
                        }
                    }
                    relevant_courier.Clear();
                    comparator = double.MaxValue;
                }
                foreach (var pickup_order in pickup_orders)
                {
                    foreach (var courier in couriers)
                    {
                        if (courier.Can_Get(pickup_order) & courier.IsBusy != true)
                        {
                            relevant_courier.Add(courier);
                        }
                    }
                    foreach (var courier in relevant_courier)
                    {
                        if (courier.way_time < comparator)
                        {
                            comparator = courier.way_time;
                        }
                    }
                    foreach (var courier in relevant_courier)
                    {
                        if (courier.way_time == comparator)
                        {
                            ColorOutput.Color_Writeline($"Заказ №{pickup_order.Order_Number} выдан курьеру {courier.Name} как самому подходящему", ConsoleColor.Green);
                            courier.taken_order = pickup_order;
                            courier.IsBusy = true;
                        }
                    }
                    relevant_courier.Clear();
                    comparator = double.MaxValue;
                }
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
