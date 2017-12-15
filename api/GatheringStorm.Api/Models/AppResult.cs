using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GatheringStorm.Api.Models
{
    public class AppResult
    {
        public AppResult(AppActionResultType result)
        {
            Result = result;
        }

        public AppResult(AppActionResultType result, string errorMessage)
        {
            Result = result;
            ErrorMessage = errorMessage;
        }

        public string ErrorMessage { get; protected set; }
        public AppActionResultType Result { get; protected set; }
    }

    public class AppResult<T> : AppResult
    {
        public AppResult(AppActionResultType result, string errorMessage) : base(result, errorMessage)
        {
        }

        public AppResult(T suceSuccessReturnValue) : base(AppActionResultType.Success)
        {
            this.SuccessReturnValue = suceSuccessReturnValue;
        }

        public T SuccessReturnValue { get; protected set; }
    }

    public enum AppActionResultType
    {
        Success = 0,
        ServerError = 1,
        RuleError = 2
    }
}
