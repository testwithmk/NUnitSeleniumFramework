using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;

namespace NunitSeleniumFramework.Utilities
{
     /**
    * Manages a single Selenium WebDriver instance.
    * Initializes browser (Chrome/Firefox/Edge), maximizes window, sets implicit wait,
    * provides access via GetWebDriver(), and handles driver cleanup.
    */
    public class CommonDriver
    {
        // WebDriver instance
        private static IWebDriver driver;


        /**Gets the current WebDriver instance.
        */
        public static IWebDriver GetWebDriver()
        {

            if (driver == null)
            {
                string browser = ConfigReader.GetValue("Browser");
                driver = InitDriver(browser);
                driver.Manage().Window.Maximize();
                driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            }
            return driver;
        }

        /**This method initializes and returns a Selenium WebDriver instance 
         * for the requested browser (chrome, firefox, or edge).
         */
        private static IWebDriver InitDriver(string browser)
        {
            switch (browser.ToLower())
            {
                case "chrome": return new ChromeDriver();
                case "firefox": return new FirefoxDriver();
                case "edge": return new EdgeDriver();
                default: throw new ArgumentException("Unsupported browser: " + browser);
            }
        }

        /**Quit the driver
        */
        public static void QuitDriver()
        {
            if (driver != null)
            {
                driver.Quit();
                driver = null;
            }
        }
    }
}
