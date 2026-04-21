using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceData.Application.Common
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Value { get; private set; }
        public string? ErrorField { get; private set; }
        public string? ErrorMessage { get; private set; }

        public static ServiceResult<T> Ok(T value) =>
            new() { IsSuccess = true, Value = value };

        public static ServiceResult<T> Fail(string field, string message) =>
            new() { IsSuccess = false, ErrorField = field, ErrorMessage = message };
    }
}
