using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Enums
{
    public enum DocumentType
    {
        Order = 1,
        Product = 2,
        Customer = 3,
        Employee = 4,
        Supplier = 5
    }

    public enum Error
    {
        None,
        NotFound,
        InvalidInput,
        Unauthorized,
        Forbidden,
        InternalServerError
    }

    public enum ProductType
    {
        Food,
        Beverage,
        Dessert,
        Snack
    }

    public enum ProductCategory
    {
        Appetizer,
        MainCourse,
        Dessert,
        Beverage,
        Snack
    }

    public enum ProductStatus
    {
        Available,
        Unavailable,
        OutOfStock
    }

    public enum RouteType
    {
        Barista,
        Kitchen
    }

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

