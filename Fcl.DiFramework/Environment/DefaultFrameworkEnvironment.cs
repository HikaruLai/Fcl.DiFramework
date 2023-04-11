using System.Diagnostics;
using System.Reflection;

namespace Fcl.DiFramework.Environment
{
    /// <summary>
    /// Default implementation about the current framework environment
    /// </summary>
    public class DefaultFrameworkEnvironment : IFrameworkEnvironment
    {
        #region Properties
        /// <summary>
        /// True if we are in a development (specifically, debuggable) environment
        /// </summary>
        public bool IsDevelopment => Assembly.GetEntryAssembly()?.GetCustomAttribute<DebuggableAttribute>()?.IsJITTrackingEnabled == true;

        /// <summary>
        /// The configuration of the environment, either Development or Production
        /// </summary>
        public string Configuration => IsDevelopment ? "Development" : "Production";
        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public DefaultFrameworkEnvironment()
        {

        }

        #endregion
    }
}
