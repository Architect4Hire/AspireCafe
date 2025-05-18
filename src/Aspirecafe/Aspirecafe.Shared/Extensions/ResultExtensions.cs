using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service;
using AspireCafe.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace AspireCafe.Shared.Extensions
{
    public static class ResultExtensions
    {
        //public static T Match<T,TModel>(this Result<TModel> result, Func<T> onSuccess, Func<Error, T> onFailure) where TModel : ServiceBaseModel
        //{
        //    if (result == null)
        //    {
        //        throw new ArgumentNullException(nameof(result));
        //    }
        //    if (onSuccess == null)
        //    {
        //        throw new ArgumentNullException(nameof(onSuccess));
        //    }
        //    if (onFailure == null)
        //    {
        //        throw new ArgumentNullException(nameof(onFailure));
        //    }
        //    {
        //        return result.IsSuccess ? onSuccess() : onFailure(result.Error);
        //    }
        //}

        public static IActionResult Match<TModel>(this Result<TModel> result) where TModel : ServiceBaseModel
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            if (result.IsSuccess)
            {
                return new OkObjectResult(result);
            }

            return result.Error switch
            {
                Error.NotFound => new NotFoundObjectResult(result),
                Error.InvalidInput => new BadRequestObjectResult(result),
                Error.Unauthorized => new UnauthorizedObjectResult(result),
                Error.Forbidden => new ForbidResult(),
                Error.InternalServerError => new ObjectResult(result) { StatusCode = 500 },
                _ => new ObjectResult(result) { StatusCode = 500 }
            };
        }
    }
}
