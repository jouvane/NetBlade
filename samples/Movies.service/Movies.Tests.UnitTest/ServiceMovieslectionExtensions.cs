using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Linq;

namespace Movies.Tests.UnitTest
{
    public static class ServiceMovieslectionExtensions
    {
        public static IServiceCollection RemoveAndAddMock<T>(this IServiceCollection services, Func<IServiceProvider, object[]> func = null)
            where T : class
        {
            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }

            services.AddScoped<T>(s =>
            {
                object[] args = func != null ? func.Invoke(s) : null;
                if (args != null)
                {
                    return new Mock<T>(args).Object;
                }
                else
                {
                    return new Mock<T>().Object;
                }
            });

            return services;
        }
    }
}
