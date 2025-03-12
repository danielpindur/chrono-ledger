using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ChronoLedger.Gateway.Swagger;

/// <summary>
/// Custom schema filter that enhances Swagger documentation for enum types
/// by showing the enum names instead of their numeric values.
/// </summary>
public class EnumSchemaFilter : ISchemaFilter
{
    /// <summary>
    /// Applies modifications to the schema for enum types to display friendly names.
    /// </summary>
    /// <param name="schema">The schema to modify</param>
    /// <param name="context">The filter context containing type information</param>
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsEnum)
        {
            schema.Enum = Enum.GetNames(context.Type)
                .Select(enumName => (IOpenApiAny)new OpenApiString(enumName))
                .ToList();
        }
    }
}