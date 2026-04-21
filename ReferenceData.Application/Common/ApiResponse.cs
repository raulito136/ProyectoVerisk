using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Application.Common
{
    public class ApiResponse<T>
    {
        public T? Data { get; set; }
        public List<ApiError> Errors { get; set; } = [];

        public static ApiResponse<T> Success(T data) => new() { Data = data };
        public static ApiResponse<T> Fail(string field, string message) =>
            new() { Errors = [new ApiError(field, message)] };
    }

    public record ApiError(string Field, string Message);
}
