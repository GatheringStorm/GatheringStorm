using System;
using System.Collections.Generic;
using System.Text;
using GatheringStorm.Api.Models;

namespace GatheringStorm.Api.Controllers
{
    public class ErrorActionResultContent
    {
        public ErrorActionResultContent(AppResult result)
        {
            Result = result.Result;
            ErrorMessage = result.ErrorMessage;
        }

        public string ErrorMessage { get; }
        public string Result { get; }
    }
}
