using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service;
using AspireCafe.Shared.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireCafe.Shared.Extensions
{
    public static class ResultExtensions
    {
        public static T Match<T,TModel>(this Result<TModel> result, Func<T> onSuccess, Func<Error, T> onFailure) where TModel : ServiceBaseModel
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            if (onSuccess == null)
            {
                throw new ArgumentNullException(nameof(onSuccess));
            }
            if (onFailure == null)
            {
                throw new ArgumentNullException(nameof(onFailure));
            }
            {
                return result.IsSuccess ? onSuccess() : onFailure(result.Error);
            }
        }
    }
}
