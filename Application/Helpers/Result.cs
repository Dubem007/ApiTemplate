using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Helpers
{
    public class Result : IResult
    {
        public Result()
        {
        }

        public string message { get; set; }

        public bool success { get; set; }

        public static IResult Fail()
        {
            return new Result { success = false };
        }

        public static IResult Fail(string message)
        {
            return new Result { success = false, message = message };
        }

        public static Task<IResult> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public static Task<IResult> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public static IResult Success()
        {
            return new Result { success = true };
        }

        public static IResult Success(string message)
        {
            return new Result { success = true, message = message};
        }

        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public Result()
        {
        }

        public T data { get; set; }


        public new static Result<T> Fail()
        {
            return new Result<T> { success = false };
        }

        public new static Result<T> Fail(string message)
        {
            return new Result<T> { success = false, message = message };
        }

        public new static Task<Result<T>> FailAsync()
        {
            return Task.FromResult(Fail());
        }

        public new static Task<Result<T>> FailAsync(string message)
        {
            return Task.FromResult(Fail(message));
        }

        public new static Result<T> Success()
        {
            return new Result<T> { success = true };
        }

        public new static Result<T> Success(string message)
        {
            return new Result<T> { success = true, message = message};
        }

        public static Result<T> Success(T data)
        {
            return new Result<T> { success = true, data = data };
        }

        public static Result<T> Success(T data, string message)
        {
            return new Result<T> { success = true, data = data, message = message};
        }

        public new static Task<Result<T>> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public new static Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));
        }

        public static Task<Result<T>> SuccessAsync(T data)
        {
            return Task.FromResult(Success(data));
        }

        public static Task<Result<T>> SuccessAsync(T data, string message)
        {
            return Task.FromResult(Success(data, message));
        }
    }
}
