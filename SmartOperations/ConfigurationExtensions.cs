using System;
using System.Configuration;
using Microsoft.Commerce.Common;
using Microsoft.Commerce.Providers;

namespace SmartOperations
{
    /// <summary>
    /// Helper extensions that duplicate some internal behaviours of the Commerce Server operations.
    /// </summary>
    public static class ConfigurationExtensions
    {
        public static T RequireAs<T>(this ConfigurationElement configuration) where T : ConfigurationElement
        {
            if (configuration == null) throw new ArgumentNullException("configuration");

            var x = configuration as T;
            if (x != null) return x;

            var message = ProviderResources.ExceptionMessages.GetMessage("UnsupportedPipelineConfigurationType", new object[]
            {
                configuration.GetType().ToString(),
                configuration.GetType(),
                typeof (T)
            });
            throw new ConfigurationErrorsException(message);
        }
    }
}