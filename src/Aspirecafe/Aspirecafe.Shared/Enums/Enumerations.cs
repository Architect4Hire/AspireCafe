using Microsoft.AspNetCore.Routing;

namespace AspireCafe.Shared.Enums
{
    public enum DocumentType
    {
        Order = 1,
        Product = 2,
        Customer = 3,
        Employee = 4,
        Supplier = 5,
        BaristaOrder = 6,
        KitchenOrder = 7
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
        Food = 1,
        Beverage = 2,
        Dessert = 3,
        Snack = 4
    }

    public enum ProductCategory
    {
        Appetizer = 1,
        MainCourse = 2,
        Dessert = 3,
        Beverage = 4,
        Snack = 5
    }

    public enum ProductStatus
    {
        Available = 1,
        Unavailable = 2,
        OutOfStock = 3
    }

    public enum RouteType
    {
        Barista = 1,
        Kitchen = 2
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
        Preparing = 2,
        Ready = 3,
        Delivered = 4,
        Cancel = 5
    }

    public enum OrderProcessStatus
    {
        Waiting = 1,
        Received = 2,
        Preparing = 3,
        Ready = 4,
        Delivered = 5,
        Cancelled = 6
    }

    public enum OrderProcessStation
    {
        Expo = 0,
        Bar = 1,
        Saute = 2,
        Grill = 3,
        Fry = 4,
        Pantry = 5,
        Vegetable = 6,
        Fish = 7,
        Rotisseur = 8
    }
}

