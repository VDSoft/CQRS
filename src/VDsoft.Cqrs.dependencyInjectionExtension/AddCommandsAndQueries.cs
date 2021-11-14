using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using VDsoft.Cqrs.Command;
using VDsoft.Cqrs.Query;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension for the <see cref="DependencyInjection"/> to easily integrate CQRS into the DI.
    /// </summary>
    public static class AddCommandsAndQueries
    {
        /// <summary>
        /// Uses the CQRS pattern defined by <see cref="VDsoft.Cqrs"/>.
        /// </summary>
        /// <param name="services">The services collection.</param>
        /// <param name="assembly">The assembly which contains the implemented commands and/or queries.</param>
        /// <returns>The <see cref="IServiceCollection"/> with all commands and queries from the provided <see cref="Assembly"/>.</returns>
        public static IServiceCollection UseCqrs(this IServiceCollection services, Assembly assembly)
        {
            var handlers = assembly.GetTypes()
                .Where(x => x.GetInterfaces().Any(i => IsHandlerInterface(i)))
                .ToList();

            foreach (var type in handlers)
            {
                AddHandler(services, type);
            }

            return services;
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType)
            {
                return false;
            }

            var typeDefinition = type.GetGenericTypeDefinition();

            return typeDefinition == typeof(ICommand<>) || typeDefinition == typeof(IQueryHandler<,>);
        }

        private static void AddHandler(IServiceCollection services, Type type)
        {
            var attributes = type.GetCustomAttributes(false);

            var pipeline = attributes
                .Select(x => ToDecorator(x))
                .Concat(new[] { type })
                .Reverse()
                .ToList();

            var interfaceType = type.GetInterfaces()
                .Single(i => IsHandlerInterface(i));
            var factory = BuildPipeline(pipeline, interfaceType);

            services.AddScoped(interfaceType, factory);
        }

        private static Func<IServiceProvider, object> BuildPipeline(List<Type> pipeline, Type interfaceType)
        {
            var ctor = pipeline
                .Select(x => GetFirstConstructor(interfaceType, x))
                .ToList();

            return BuildFactoryFunction(ctor);
        }

        private static ConstructorInfo GetFirstConstructor(Type interfaceType, Type x)
        {
            var type = x.IsGenericType ? x.MakeGenericType(interfaceType.GenericTypeArguments) : x;
            return type.GetConstructors().Single();
        }

        private static Func<IServiceProvider, object> BuildFactoryFunction(List<ConstructorInfo> ctor)
        {
            return provider =>
            {
                object current = null!;

                foreach (var constructor in ctor)
                {
                    var parameterInfo = constructor.GetParameters().ToList();
                    object[] parameters = GetParameters(parameterInfo, constructor, provider);

                    current = constructor.Invoke(parameters);
                }

                return current;
            };
        }

        private static object[] GetParameters(List<ParameterInfo> parameterInfos, object current, IServiceProvider serviceProvider)
        {
            var result = new object[parameterInfos.Count];

            for (int i = 0; i < parameterInfos.Count; i++)
            {
                result[i] = GetParameter(parameterInfos[i], current, serviceProvider);
            }

            return result;
        }

        private static object GetParameter(ParameterInfo parameterInfo, object current, IServiceProvider serviceProvider)
        {
            var parameterType = parameterInfo.ParameterType;

            if (IsHandlerInterface(parameterType))
            {
                return current;
            }

            object service = serviceProvider.GetService(parameterType) ?? throw new ArgumentException($"Type {parameterType} not found."); ;

            return service;
        }

        private static Type ToDecorator(object attribute)
        {
            return null!;
        }
    }
}
