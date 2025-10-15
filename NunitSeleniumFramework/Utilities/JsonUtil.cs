using Newtonsoft.Json;

namespace NunitSeleniumFramework.Utilities
{
    /**
    * Provides utility methods to read and deserialize JSON files into strongly-typed objects.
    * Used to load test data from JSON files for automated tests.
    */
    public class JsonUtil
    {
        public static T ReadJsonFile<T>(string relativePath)
        {
            string path = Path.Combine(TestContext.CurrentContext.TestDirectory, relativePath);

            if (!File.Exists(path))
                throw new FileNotFoundException($"JSON file not found: {path}");

            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
