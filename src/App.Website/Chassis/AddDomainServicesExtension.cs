using System.Reflection;
using App.Domain;
using App.Infrastructure;
using FluentValidation;

namespace App.Website.Chassis;

public static class AddDomainServicesExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddTransient<ISystemClock, SystemClock>();
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

        services.AddTransient<IJobRepository, InMemoryJobRepository>();
        services.AddSingleton<JobsDatastore>();

        RegisterTypes(typeof(IQuery<>), type => services.AddTransient(type), typeof(AllJobsQuery).Assembly);

        RegisterTypes(typeof(ICommandHandler<>), type =>
        {
            var interfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.ImplementsOpenGenericInterface(typeof(ICommandHandler<>)));
            services.AddTransient(interfaceType, type);

        }, typeof(ICommandHandler<>).Assembly);

        RegisterTypes(typeof(ICommandHandler<,>), type =>
        {
            var interfaceType = type.GetInterfaces().Single(x => x.IsGenericType && x.ImplementsOpenGenericInterface(typeof(ICommandHandler<,>)));
            services.AddTransient(interfaceType, type);
        }, typeof(ICommandHandler<>).Assembly);


        var scanResults = AssemblyScanner.FindValidatorsInAssemblyContaining(typeof(CreateJobCommand));
        foreach (var scan in scanResults)
        {
            services.AddTransient(scan.InterfaceType, scan.ValidatorType);
        }

        return services;
    }

    public static void RegisterTypes(this Type interfaceType, Action<Type> action, params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var types = assembly.DefinedTypes
                .Where(x => x.IsClass && !x.IsAbstract && x.ImplementsOpenGenericInterface(interfaceType))
                .ToList();

            foreach (var type in types)
            {
                action(type);
            }
        }
    }

    public static bool ImplementsOpenGenericInterface(this Type candidateType, Type openGenericInterfaceType)
    {
        return
            candidateType.Equals(openGenericInterfaceType) ||
            (candidateType.IsGenericType && candidateType.GetGenericTypeDefinition().Equals(openGenericInterfaceType)) ||
            candidateType.GetInterfaces().Any(i => i.IsGenericType && i.ImplementsOpenGenericInterface(openGenericInterfaceType));
    }
}
