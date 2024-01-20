using App.Authentication;
using App.Core;
using App.Domain;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace App.Infrastructure;

public static class DomainServiceExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<ISystemClock, SystemClock>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddTransient<IQueryDispatcher, QueryDispatcher>();
        services.AddTransient<ICommandDispatcher, CommandDispatcher>();
        services.AddTransient<IDbConnectionFactory, DbConnectionFactory>();

        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddTransient<IAuthenticationClaimsBuilder, AuthenticationClaimsBuilder>();
        services.AddTransient<ClaimsBuilder>();
        services.AddTransient<IPermissionService, AlwaysAdminPermissionService>();

        // misc
        services.AddSingleton<JobsDatastore>();
        services.AddTransient<IJobRepository, InMemoryJobRepository>();

        // add all queries
        RegisterTypes(typeof(IQuery<>), type => services.AddTransient(type), typeof(AllJobsQuery).Assembly);

        // add all commands
        RegisterTypes(
            typeof(ICommandHandler<>),
            type =>
            {
                var inf = type.GetInterfaces()
                    .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<>));
                services.AddTransient(inf, type);
            },
            typeof(CommandDispatcher).Assembly);
        RegisterTypes(
            typeof(ICommandHandler<,>),
            type =>
            {
                var inf = type.GetInterfaces()
                    .Single(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICommandHandler<,>));
                services.AddTransient(inf, type);
            },
            typeof(CommandDispatcher).Assembly);

        // add fluent validations
        var scanResults = AssemblyScanner.FindValidatorsInAssemblies(new[] { typeof(AllJobsQuery).Assembly, typeof(CommandDispatcher).Assembly });
        foreach (var scan in scanResults)
        {
            services.AddTransient(scan.InterfaceType, scan.ValidatorType);
        }

        SqlMapper.AddTypeHandler(new DapperDateTimeHandler());

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
