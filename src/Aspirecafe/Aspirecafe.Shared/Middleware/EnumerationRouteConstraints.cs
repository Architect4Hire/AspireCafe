using AspireCafe.Shared.Enums;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AspireCafe.Shared.Middleware
{
    public class OrderProcessStationRouteConstraint : IRouteConstraint, IParameterPolicy
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var candidate = values[routeKey]?.ToString();
            return System.Enum.TryParse(candidate, out OrderProcessStation result);
        }
    }

    public class OrderProcessStatusRouteConstraint : IRouteConstraint, IParameterPolicy
    {
        public bool Match(HttpContext httpContext, IRouter route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection)
        {
            var candidate = values[routeKey]?.ToString();
            return System.Enum.TryParse(candidate, out OrderProcessStatus result);
        }
    }
}
