using System;
using System.Collections.Generic;
using System.Text;
using GatheringStorm.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GatheringStorm.Api.Controllers
{
    public interface IControllerUtility
    {
        IActionResult GetResult(AppActionResult appActionResult);
        IActionResult GetResult<T>(AppActionResult<T> appActionResult);
    }

    public class ControllerUtility : IControllerUtility
    {
        public IActionResult GetResult(AppActionResult appActionResult)
        {
            if (appActionResult.Result == AppActionResultType.Success)
            {
                return new OkResult();
            }

            return new ObjectResult(null)
            {
                StatusCode = 418
            };
        }

        public IActionResult GetResult<T>(AppActionResult<T> appActionResult)
        {
            if (appActionResult.Result == AppActionResultType.Success)
            {
                return new OkObjectResult(appActionResult.SuccessReturnValue);
            }

            return new ObjectResult(appActionResult.ErrorMessage)
            {
                StatusCode = 418
            };
        }
    }
}
