using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

namespace LawGuardPro.API.Filters;

public class FileUploadOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var fileUploadParams = context.MethodInfo
            .GetParameters()
            .Where(p => p.ParameterType == typeof(IFormFile));

        if (fileUploadParams.Any())
        {
            operation.Parameters.Clear();
            operation.RequestBody = new OpenApiRequestBody
            {
                Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = {
                                { "chunk", new OpenApiSchema { Type = "string", Format = "binary" } },
                                { "fileName", new OpenApiSchema { Type = "string" } },
                                { "chunkIndex", new OpenApiSchema { Type = "integer", Format = "int32" } },
                                { "totalChunks", new OpenApiSchema { Type = "integer", Format = "int32" } }
                            },
                            Required = new HashSet<string> { "chunk", "fileName", "chunkIndex", "totalChunks" }
                        }
                    }
                }
            };
        }
    }
}


