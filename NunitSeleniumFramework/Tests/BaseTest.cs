using AventStack.ExtentReports;
using NunitSeleniumFramework.Utilities;

namespace NunitSeleniumFramework.Tests
{
    /**
    * BaseTest handles setup/teardown for all tests.
    * Integrates WebDriver, ExtentReports, and screenshots.
    */
    [TestFixture]
    public class BaseTest : CommonDriver
    {
        protected ExtentReports extent; // ExtentReports instance
        protected ExtentTest test;  // Individual test instance

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            //Initialize ExtentReports
            extent = ExtentReportManager.GetInstance();
        }
        [SetUp]
        public void SetUp()
        {
            // Initialize WebDriver before each test
            GetWebDriver();

            // Start a new test in ExtentReports
            test = extent.CreateTest(TestContext.CurrentContext.Test.Name);
        }

        /** Runs after each test - take screenshot if failed and mark test status */
        [TearDown]
        public void TearDown()
        {
            var outcome = TestContext.CurrentContext.Result.Outcome.Status;
            string testName = TestContext.CurrentContext.Test.Name;

            if (outcome == NUnit.Framework.Interfaces.TestStatus.Failed)
            {
                // Capture screenshot
                string screenshotPath = ScreenshotHelper.Capture(GetWebDriver(), testName);

                if (!string.IsNullOrEmpty(screenshotPath))
                {
                    test.Fail("Test Failed")
                        .AddScreenCaptureFromPath(screenshotPath); // Attach screenshot
                }

                test.Fail(TestContext.CurrentContext.Result.Message);
            }
            else if (outcome == NUnit.Framework.Interfaces.TestStatus.Passed)
            {
                test.Pass("Test Passed");
            }

            // Quit driver after each test
            QuitDriver();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            // Save Extent Report
            extent.Flush();

            // Get the path to the report
            string reportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", "ExtentReport.html");

            // Open the report automatically in default browser
            if (File.Exists(reportPath))
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = reportPath,
                    UseShellExecute = true // important to open with default browser
                });
            }
        }
    }
}