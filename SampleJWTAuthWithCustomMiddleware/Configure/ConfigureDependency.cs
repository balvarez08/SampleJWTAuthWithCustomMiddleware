using Data;
using Microsoft.Extensions.DependencyInjection;
using Services;

namespace SampleJWTAuthWithCustomMiddleware.Configure
{
    public static class ConfigureDependency
    {
        public static void ConfigureDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IUserRepository, UserRepository>();

        }
    }
}
