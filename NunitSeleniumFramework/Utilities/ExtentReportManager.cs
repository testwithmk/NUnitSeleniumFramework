using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports.Reporter.Config;

namespace NunitSeleniumFramework.Utilities
{
    /** 
     * Class to manage ExtentReports instance for NUnit tests.
     * Creates and configures the report only once and returns the same instance.
    */
    public class ExtentReportManager
    {
        // Single instance of ExtentReports
        private static ExtentReports extent;

        /** Get or create the ExtentReports instance*/
        public static ExtentReports GetInstance()
        {
            if (extent == null)
            {
                // Define the folder and file path for the report
                string reportDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
                // Create Reports folder if not exists
                Directory.CreateDirectory(reportDir); 

                string reportPath = Path.Combine(reportDir, "ExtentReport.html");

                // Create a Spark reporter (modern HTML report)
                var sparkReporter = new ExtentSparkReporter(reportPath);

                // Optional: Configure report appearance
                sparkReporter.Config.DocumentTitle = "Automation Test Report";
                sparkReporter.Config.ReportName = "Extent Report";
                sparkReporter.Config.Theme = Theme.Standard;

                // Create ExtentReports instance and attach the reporter
                extent = new ExtentReports();
                extent.AttachReporter(sparkReporter);

                // Optional: Add system info
                extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
            }

            return extent;
        }
    }
}
