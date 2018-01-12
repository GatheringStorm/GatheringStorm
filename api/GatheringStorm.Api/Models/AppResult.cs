using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace GatheringStorm.Api.Models
{
    public abstract class AppResult
    {
        private string result;

        public string ErrorMessage { get; protected set; }

        public string Result
        {
            get => this.result;
            protected set
            {
                if (!value.IsValidAppActionResultType())
                {
                    throw new ArgumentException("Value is not a valid appActionResultType: " + value, nameof(Result));
                }

                this.result = value;
            }
        }
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

        public static VoidAppResult Error(string result, string errorMessage)
        {
            return new VoidAppResult
            {
                Result = result,
                ErrorMessage = errorMessage
            };
        }

        public static VoidAppResult Error(ErrorPreset errorPreset)
        {
            switch (errorPreset)
            {
                case ErrorPreset.NotYourTurn:
                    return VoidAppResult.Error(AppActionResultType.UserError, "It's not your turn.");
                case ErrorPreset.NotAParticipant:
                    return VoidAppResult.Error(AppActionResultType.UserError, "You are not a participant of this game.");
                default: // OnLoadingData
                    return VoidAppResult.Error(AppActionResultType.ServerError, "There was an error while loading game data.");   
            }
        }
    }

    public enum ErrorPreset
    {
        NotAParticipant,
        NotYourTurn,
        OnLoadingData
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

        public static AppResult<T> Error(string result, string errorMessage)
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

    public static class AppActionResultType
    {
        public static string Success { get; } = "success";
        public static string UserError { get; } = "userError";
        public static string ServerError { get; } = "serverError";
        public static string RuleError { get; } = "ruleError";

        private static readonly List<string> resultTypes = new List<string>
        {
            Success,
            UserError,
            ServerError,
            RuleError
        };

        public static bool IsValidAppActionResultType(this string resultType)
        {
            return resultType == null || resultTypes.Contains(resultType);
        }
    }
}
