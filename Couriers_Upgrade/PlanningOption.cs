using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Courier_Upgrade
{
    class PlanningOption
    {
        public Courier Courier { get; set; }
        public Order Order { get; set; }
        public decimal CourierPrice { get; set; }
        public double EstimatedTimeOfExecution { get; set; }
        public decimal Profit { get { return Order.Price - CourierPrice; } }

    }
}
