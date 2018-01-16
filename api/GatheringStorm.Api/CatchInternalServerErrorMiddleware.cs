using System;
using System.Threading.Tasks;
using GatheringStorm.Api.Controllers;
using GatheringStorm.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace GatheringStorm.Api
{
    public class CatchInternalServerErrorMiddleware
    {
        private readonly RequestDelegate next;

        public CatchInternalServerErrorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.Headers.Add("Content-Type", "application/json");
                httpContext.Response.StatusCode = 500;
                await httpContext.Response.WriteAsync(JsonConvert.SerializeObject(new ErrorActionResultContent(
                        VoidAppResult.Error(AppActionResultType.ServerError,
                            $"Internal server error\nException type: {ex.GetType().FullName}\nException message: '{ex.Message}'")
                    )));
            }
        }
    }
}
