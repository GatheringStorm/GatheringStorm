using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Text;

namespace GatheringStorm.Api.Models
{
    public abstract class AppResult
    {
        public string ErrorMessage { get; protected set; }

        public AppActionResultType Result { get; set; }
    }

    public class VoidAppResult : AppResult
    {
        protected VoidAppResult()
        {
        }

        public static VoidAppResult Success()
        {
            return new VoidAppResult
            {
                Result = AppActionResultType.Success
            };
        }

        public static VoidAppResult Error(AppActionResultType result, string errorMessage)
        {
            return new VoidAppResult
            {
                Result = result,
                ErrorMessage = errorMessage
            };
        }
    }

    public class AppResult<T> : AppResult
    {
        protected AppResult()
        {
        }

        public static AppResult<T> Success(T value)
        {
            return new AppResult<T>
            {
                Result = AppActionResultType.Success,
                SuccessReturnValue = value
            };
        }

        public static AppResult<T> Error(AppActionResultType result, string errorMessage)
        {
            return new AppResult<T>
            {
                Result = result,
                ErrorMessage = errorMessage
            };
        }

        public VoidAppResult GetVoidAppResult()
        {
            return this.Result == AppActionResultType.Success
                ? VoidAppResult.Success()
                : VoidAppResult.Error(this.Result, this.ErrorMessage);
        }

        public T SuccessReturnValue { get; protected set; }
    }

    public enum AppActionResultType
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "userError")]
        UserError,
        [EnumMember(Value = "serverError")]
        ServerError,
        [EnumMember(Value = "ruleError")]
        RuleError
    }
}
