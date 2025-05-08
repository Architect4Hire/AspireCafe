namespace AspireCafe.ProductApiDomainLayer.Managers.Models.Enums
{
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
}
