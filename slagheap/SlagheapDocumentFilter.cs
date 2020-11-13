using System.Collections.Generic;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace slagheap
{
    public class SlagheapDocumentFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Servers = new List<OpenApiServer>()
            {
                new OpenApiServer() {Url = "https://slagheapappservice.azurewebsites.net"}
            };
        }
    }
}