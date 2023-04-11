using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Exceptions;
using Fcl.DiFramework.Construction;

namespace Fcl.DiFramework
{
    /// <summary>
    /// Extension methods for the framework
    /// </summary>
    public static class FrameworkExtensions
    {
        #region Configuration

        /// <summary>
        /// Configures a framework construction in the default way
        /// </summary>
        /// <param name="construction">The construction to configure</param>
        /// <param name="configure">The custom configuration action</param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultConfiguration(this FrameworkConstruction construction, Action<IConfigurationBuilder> configure = null)
        {
            // Create our configuration sources
            var configurationBuilder = new ConfigurationBuilder()
                // Add environment variables
                .AddEnvironmentVariables();

            // Add file based configuration
            // Set base path for Json files as the startup location of the application
            //configurationBuilder.SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            // Add application settings json files
            configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            configurationBuilder.AddJsonFile($"appsettings.{construction.Environment.Configuration}.json", optional: true, reloadOnChange: true);

            // Let custom configuration happen
            configure?.Invoke(configurationBuilder);

            // Inject configuration into services
            var configuration = configurationBuilder.Build();

            construction.Services.AddSingleton<IConfiguration>(configuration);

            // Set the construction Configuration
            construction.UseConfiguration(configuration);

            // Chain the construction
            return construction;
        }

        /// <summary>
        /// Configures a framework construction using the provided configuration
        /// </summary>
        /// <param name="construction">The construction to configure</param>
        /// <param name="configure">The configuration</param>
        /// <returns></returns>
        public static FrameworkConstruction AddConfiguration(this FrameworkConstruction construction, IConfiguration configuration)
        {
            // Add specific configuration
            construction.UseConfiguration(configuration);

            // Add configuration to services
            construction.Services.AddSingleton(configuration);

            // Chain the construction
            return construction;
        }

        #endregion

        #region Services

        /// <summary>
        /// Injects all of the default services used by framework for a quicker and cleaner setup
        /// </summary>
        /// <param name="construction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultServices(this FrameworkConstruction construction)
        {
            // Add exception handler
            //construction.AddDefaultExceptionHandler();

            // Add default logger
            construction.AddDefaultLogger();

            // Chain the construction
            return construction;
        }

        /// <summary>
        /// Injects the default logger into the framework construction
        /// </summary>
        /// <param name="construction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultLogger(this FrameworkConstruction construction)
        {
            var configuration = construction.Configuration;
            // Add logging as default
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.File(
                    configuration["Logging:LogFileLocation"],
                    rollingInterval: RollingInterval.Day,
                    rollOnFileSizeLimit: false,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj} {NewLine}"
                )
                .MinimumLevel.Debug()
                .CreateLogger();

            var loggerFactory = new LoggerFactory();
            construction.Services.AddSingleton(loggerFactory.AddSerilog()).AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            // Chain the construction
            return construction;
        }

        /// <summary>
        /// Injects the default exception handler into the framework construction
        /// </summary>
        /// <param name="construction">The construction</param>
        /// <returns></returns>
        public static FrameworkConstruction AddDefaultExceptionHandler(this FrameworkConstruction construction)
        {
            // Bind a static instance of the BaseExceptionHandler
            //construction.Services.AddSingleton<IExceptionHandler>(new BaseExceptionHandler());

            // Chain the construction
            return construction;
        }

        #endregion
    }
}
