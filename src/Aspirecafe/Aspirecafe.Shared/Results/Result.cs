using AspireCafe.Shared.Enums;
using AspireCafe.Shared.Models.Service;

namespace AspireCafe.Shared.Results
{
    public class Result<T> where T : ServiceBaseModel
    {
        private Result(bool isSuccess, Error error, List<string>? messages, T? data)
        {
            if (isSuccess && error != Error.None ||
                !isSuccess && error == Error.None)
            {
                throw new ArgumentException("Invalid error", nameof(error));
            }

            IsSuccess = isSuccess;
            Error = error;
            Messages = messages ?? new List<string>();
            Data = data;
        }

        public bool IsSuccess { get; }

        public bool IsFailure => !IsSuccess;

        public List<string> Messages { get; set; }

        public Error Error { get; }

        public T? Data { get; set; }

        public static Result<T> Success(T data) => new(true, Error.None, null, data);

        public static Result<T> Failure(Error error, List<string>? messages) => new(false, error, messages, null);
    }
}
