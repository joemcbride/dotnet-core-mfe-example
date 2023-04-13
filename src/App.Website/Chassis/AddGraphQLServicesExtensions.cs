using System.Reflection;
using System.Text;
using App.Website.Jobs;
using App.Website.Schema;
using GraphQL;
using GraphQL.Execution;
using GraphQL.SystemTextJson;
using GraphQL.Types;
using GraphQL.Validation;

namespace App.Website.Chassis;

public static class AddGraphQLServicesExtensions
{
    public static IServiceCollection AddGraphQLServices(this IServiceCollection services)
    {
        services.AddTransient<IDocumentExecuter, DocumentExecuter>();
        services.AddTransient<IDocumentBuilder, GraphQLDocumentBuilder>();
        services.AddTransient<IDocumentValidator, DocumentValidator>();
        services.AddTransient<IGraphQLSerializer, GraphQLSerializer>();
        services.AddTransient<IGraphQLTextSerializer, GraphQLSerializer>();

        var (schemaTypes, schemaDefinitions) = FindGraphTypes(typeof(QueryGraphType).Assembly);

        foreach (var type in schemaTypes)
        {
            services.AddTransient(type);
        }

        services.AddTransient<ISchema>(s =>
        {
            return GraphQL.Types.Schema.For(schemaDefinitions, _ =>
            {
                _.ServiceProvider = s;

                foreach (var type in schemaTypes)
                {
                    _.Types.Include(type);
                }
            });
        });

        return services;
    }

    public static (List<Type>, string) FindGraphTypes(params Assembly[] assemblies)
    {
        List<Type> schemaTypes = new();

        StringBuilder schemaBuilder = new();

        foreach (var assembly in assemblies)
        {
            var types = assembly.DefinedTypes
                .Where(x => x.IsClass && !x.IsAbstract && x.GetCustomAttribute<GraphQLSchemaMetadataAttribute>() != null)
                .Select(type => (Type: type, Meta: type.GetCustomAttribute<GraphQLSchemaMetadataAttribute>()))
                .ToList();

            foreach (var type in types)
            {
                schemaTypes.Add(type.Type);
                schemaBuilder.AppendLine(type.Meta.Schema);
            }
        }

        return (schemaTypes, schemaBuilder.ToString());
    }
}
