using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Moq;

namespace Movies.Tests.UnitTest
{
    public static class ServiceMovieslectionExtensions
    {
        public static IServiceMovieslection RemoveAndAddMock<T>(this IServiceMovieslection services, Func<IServiceProvider, object[]> func = null)
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
