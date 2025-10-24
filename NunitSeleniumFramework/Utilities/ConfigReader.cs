using Microsoft.Extensions.Configuration;

namespace NunitSeleniumFramework.Utilities
{
    /**
    * Reads configuration values from appsettings.json.
    * Initializes IConfiguration and provides access via GetValue(key) method.
    */
    public class ConfigReader
    {
        private static IConfiguration config;

        /** Static constructor to initialize configuration on first access.
         */
        static ConfigReader()
        {
            config = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true).Build();

        }

        /** Method to get a configuration value from the AppSettings section.
         * @param key : the key of the setting to retrieve
         * @return : associated value
         */
        public static string GetValue(string key)
        {
            return config[$"AppSettings:{key}"];
        }
    }
}
