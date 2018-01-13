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

        public bool IsErrorResult
        {
            get
            {
                return this.Result != AppActionResultType.Success;
            }
        }
    }

    public class VoidAppResult : AppResult
    {
        protected VoidAppResult()
        {
        }

        public AppResult<T> GetErrorAppResult<T>()
        {
            if (this.Result == AppActionResultType.Success)
            {
                return AppResult<T>.Error(AppActionResultType.ServerError, "Error when converting AppResult.");
            }
            return AppResult<T>.Error(this.Result, this.ErrorMessage);
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

        public static VoidAppResult Error(ErrorPreset errorPreset)
        {
            switch (errorPreset)
            {
                case ErrorPreset.NotYourTurn:
                    return VoidAppResult.Error(AppActionResultType.UserError, "It's not your turn.");
                case ErrorPreset.NotAParticipant:
                    return VoidAppResult.Error(AppActionResultType.UserError, "You are not a participant of this game.");
                case ErrorPreset.InvalidTargets:
                    return VoidAppResult.Error(AppActionResultType.UserError, "Some of the targets were invalid targets.");
                default: // OnLoadingData
                    return VoidAppResult.Error(AppActionResultType.ServerError, "There was an error while loading game data.");
            }
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

    public enum ErrorPreset
    {
        OnLoadingData,
        NotYourTurn,
        NotAParticipant,
        InvalidTargets
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
