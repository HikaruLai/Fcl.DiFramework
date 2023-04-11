using Microsoft.Extensions.DependencyInjection;
using Fcl.DiFramework.Construction;

namespace Fcl.DiFramework
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Used in a hosted environment when using an existing set of services and configuration, such as 
        /// in an ASP.Net Core environment
        /// </summary>
        /// <param name="services">The services to use</param>
        /// <returns></returns>
        public static FrameworkConstruction AddFramework(this IServiceCollection services)
        {
            // Add the services into the framework
            Framework.Construction.UseHostedServices(services);

            // Return construction for chaining
            return Framework.Construction;
        }
    }
}
