using Microsoft.Extensions.Configuration;

namespace Sync.Common.ConfigurationSettings
{
    public static class ConfigurationRegistery
    {
        public static IConfigurationBuilder RegisterConfiguration<T>(this ConfigurationBuilder configuration) where T : class
        {
            return configuration.SetBasePath(Environment.CurrentDirectory)
            .AddUserSecrets<T>()
            .AddEnvironmentVariables();
        }
    }
}
