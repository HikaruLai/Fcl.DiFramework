using System;
using Microsoft.Extensions.Hosting;
using Fcl.DiFramework.Construction;

namespace Fcl.DiFramework
{
    public static class WebHostBuilderExtensions
    {
        /// <summary>
        /// Adds the framework construct to the ASP.Net Core application
        /// </summary>
        /// <param name="builder">The web host builder</param>
        /// <param name="configure">Custom action to configure the framework</param>
        /// <returns></returns>
        public static IHostBuilder UseDiFramework(this IHostBuilder builder, Action<FrameworkConstruction> configure = null)
        {
            builder.ConfigureServices((context, services) =>
            {
                // Construct a hosted framework
                Framework.Construct<DefaultFrameworkConstruction>().Build();

                // Setup this service collection to be used by framework 
                services.AddFramework()
                        .AddDefaultConfiguration()
                        .AddDefaultServices()
                        .AddConfiguration(context.Configuration)
                ;
                // Fire off construction configuration
                configure?.Invoke(Framework.Construction);
            });

            // Return builder for chaining
            return builder;
        }
    }
}
