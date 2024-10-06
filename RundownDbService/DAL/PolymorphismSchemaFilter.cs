using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace RundownDbService.DAL
{
    public class PolymorphismSchemaFilter<TBase> : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type == typeof(TBase))
            {
                var derivedTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(x => x.GetTypes())
                    .Where(x => typeof(TBase).IsAssignableFrom(x) && x != typeof(TBase))
                    .ToList();

                schema.Discriminator = new OpenApiDiscriminator { PropertyName = "detailType" };
                schema.OneOf = new List<OpenApiSchema>();

                foreach (var derivedType in derivedTypes)
                {
                    // Generer skemaet for den afledte type og tilføj det til SchemaRepository
                    var derivedSchema = context.SchemaGenerator.GenerateSchema(derivedType, context.SchemaRepository);

                    // Tilføj en reference til det afledte skema i OneOf-listen
                    schema.OneOf.Add(new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Id = derivedSchema.Reference.Id,
                            Type = ReferenceType.Schema
                        }
                    });
                }
            }
        }
    }
}
