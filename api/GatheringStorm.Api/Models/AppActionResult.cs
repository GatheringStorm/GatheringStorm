using System;
using System.Collections.Generic;
using System.Text;

namespace GatheringStorm.Api.Models
{
    public class AppActionResult<T>
    {
        public AppActionResult(AppActionResultType type, string errorMessage)
        {
            this.Type = type;
            this.ErrorMessage = errorMessage;
        }

        public AppActionResult(T suceSuccessReturnValue)
        {
            this.Type = AppActionResultType.Success;
            this.SuccessReturnValue = suceSuccessReturnValue;
        }

        public AppActionResultType Type { get; set; }
        public string ErrorMessage { get; set; }
        public T SuccessReturnValue { get; set; }
    }

    public enum AppActionResultType
    {
        Success = 0,
        ServerError = 1,
        RuleError = 2
    }
}
