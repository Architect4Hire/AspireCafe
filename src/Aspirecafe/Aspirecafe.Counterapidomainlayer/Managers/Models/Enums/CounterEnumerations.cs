using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.CounterApiDomainLayer.Managers.Models.Enums
{
    public enum OrderType
    {
        DineIn = 1,
        TakeAway = 2,
        Delivery = 3
    }

    public enum PaymentMethod
    {
        Cash = 1,
        Card = 2,
        MobilePayment = 3
    }

    public enum PaymentStatus
    {
        Paid = 1,
        Unpaid = 2,
        Refunded = 3
    }

    public enum OrderStatus
    {
        Pending = 1,
        Completed = 2,
        Cancelled = 3
    }
}
