using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GatheringStorm.Api.Auth
{
    public class ValidateGoogleLoginAttribute : TypeFilterAttribute
    {
        public ValidateGoogleLoginAttribute() : base(typeof(Implementation))
        {
        }

        private class Implementation : IAsyncActionFilter
        {
            private readonly IConfiguration configuration;
            private readonly ILoginManager loginManager;

            private const string BearerPrefix = "Bearer ";

            public Implementation(IConfiguration configuration, ILoginManager loginManager)
            {
                this.configuration = configuration;
                this.loginManager = loginManager;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                var authHeader = context.HttpContext.Request.Headers["Authorization"];
                if (authHeader.Count == 0 || authHeader.ToString().Length < BearerPrefix.Length)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }

                var idToken = authHeader.ToString().Remove(0, BearerPrefix.Length);

                using (var client = new HttpClient())
                {
                    var tokenInfoResponse = await client.GetAsync("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + idToken);
                    if (!tokenInfoResponse.IsSuccessStatusCode)
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    var tokenInfo = JsonConvert.DeserializeObject<TokenInfo>(await tokenInfoResponse.Content.ReadAsStringAsync());
                    // Validate that the token is for this application
                    if (tokenInfo.Aud != this.configuration["GatheringStormGoogleClientId"])
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }

                    await this.loginManager.SetLoggedInUser(tokenInfo.Email);
                }

                await next();
            }
        }

        private class TokenInfo
        {
            public string Iss { get; set; }
            public string Aud { get; set; }
            public string Email { get; set; }
        }
    }
}