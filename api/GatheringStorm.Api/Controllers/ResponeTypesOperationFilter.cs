using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GatheringStorm.Api.Controllers
{
    public class ResponseTypesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Responses.Add("401", new Response
            {
                Description = "Unauthorized",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(ErrorActionResultContent))
            });
            operation.Responses.Add("500", new Response
            {
                Description = "Fatal, unhandled exception",
                Schema = context.SchemaRegistry.GetOrRegister(typeof(ErrorActionResultContent))
            });
        }
    }
}
