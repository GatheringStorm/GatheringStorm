using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GatheringStorm.Api.Models
{
    public class AppActionResult
    {
        public AppActionResult(AppActionResultType result)
        {
            Result = result;
        }

        public AppActionResultType Result { get; }
    }

    public class AppActionResult<T>
    {
        public AppActionResult(AppActionResultType result, string errorMessage)
        {
            if (result == AppActionResultType.Success)
            {
                throw new InvalidEnumArgumentException(nameof(result), (int)result, typeof(AppActionResultType));
            }

            this.Result = result;
            this.ErrorMessage = errorMessage;
        }

        public AppActionResult(T suceSuccessReturnValue)
        {
            this.Result = AppActionResultType.Success;
            this.SuccessReturnValue = suceSuccessReturnValue;
        }

        public AppActionResultType Result { get; }
        public string ErrorMessage { get; }
        public T SuccessReturnValue { get; }
    }

    public enum AppActionResultType
    {
        Success = 0,
        ServerError = 1,
        RuleError = 2
    }
}
