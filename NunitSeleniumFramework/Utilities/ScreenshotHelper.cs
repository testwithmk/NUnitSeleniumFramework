using OpenQA.Selenium;

namespace NunitSeleniumFramework.Utilities
{
    /**
    * Provides utility methods to capture screenshots during test execution.
    * Screenshots are saved with a timestamp and can be used in reports for both passed and failed tests.
    */
    public class ScreenshotHelper
    {
        public static string Capture(IWebDriver driver, string testName)
        {
            string screenshotsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Screenshots");
            Directory.CreateDirectory(screenshotsDir);

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string path = Path.Combine(screenshotsDir, $"{testName}_{timestamp}.png");

            Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
            ss.SaveAsFile(path);

            return path;
        }
    }
}
