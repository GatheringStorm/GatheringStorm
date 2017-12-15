using System;
using System.Collections.Generic;
using System.Text;
using GatheringStorm.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GatheringStorm.Api.Controllers
{
    public interface IControllerUtility
    {
        IActionResult GetActionResult(AppResult appResult);
        IActionResult GetActionResult<T>(AppResult<T> appResult);
    }

    public class ControllerUtility : IControllerUtility
    {
        public IActionResult GetActionResult(AppResult appResult)
        {
            return appResult.Result == AppActionResultType.Success
                ? new OkResult()
                : GetErrorResult(appResult);
        }

        public IActionResult GetActionResult<T>(AppResult<T> appResult)
        {
            return appResult.Result == AppActionResultType.Success
                ? new OkObjectResult(appResult.SuccessReturnValue)
                : GetErrorResult(appResult);
        }

        private static IActionResult GetErrorResult(AppResult appResult)
        {
            return new ObjectResult(new ErrorActionResultContent(appResult))
            {
                StatusCode = 418
            };
        }
    }
}
