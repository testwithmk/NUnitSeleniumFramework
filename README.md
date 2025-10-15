\## Selenium WebDriver Automation Framework



Overview: 

This is a Selenium WebDriver automation framework built with C#, NUnit, and ExtentReports for end-to-end testing of a web application.

The web portal runs in Docker containers, with configuration managed through YAML for consistent test environments. 

The framework supports:

\- Page Object Model (POM) design

\- JSON-driven test data

\- Logging and reporting with ExtentReports

\- Automatic screenshots on test pass/failure

\- Reusable utilities for JSON handling, screenshots, and waits

\- Test cleanup methods for dynamic data



\## Project Structure

NunitSeleniumFramework/

\- ├──

\- ├── Pages/         # Page classes (POM)

\- ├── Models/        # Data models for test data

\- ├── Tests/         # NUnit test classes

\- ├── Utilities/     # Helper classes (JSON, screenshots, reporting, waits)

\- ├── TestData/      # JSON test files

\- ├── Screenshots/   # Auto-generated screenshots

\- ├── Config/        # appsettings.json / YAML configurations

\- └── README.md



Prerequisites

\- Visual Studio 2022 or later

\- .NET 6.0 or later

\- ChromeDriver or corresponding browser driver installed

\- NUnit 3.x

\- Selenium.WebDriver and Selenium.Support packages

\- ExtentReports package



Setup \& Installation

1\. Clone the repository:  git clone <repository-url>

2\. Open the solution in Visual Studio

3\. Restore NuGet packages: Tools -> NuGet Package Manager -> Restore

4\. Ensure JSON test data files are present in TestData/ directory



Running Tests

1\. Open Test Explorer in Visual Studio.

2\. Run all tests or select individual tests.

3\. Test execution logs and screenshots will be generated automatically.

4\. ExtentReports will be saved in the output directory.



Features

\- POM Architecture: Encapsulates page elements and actions to enhance maintainability.

\- Reusable Test Data: Test data loaded dynamically from JSON using JsonUtil.

\- Screenshots: Captured automatically using ScreenshotHelper on pass/failure.

\- Reporting: Detailed logs and results generated with ExtentReports.

\- Dynamic Cleanup: Temporary data (e.g., certifications, education entries) is removed after test execution.



Utilities

\- JsonUtil: Reads and deserializes JSON files

\- ScreenshotHelper: Captures screenshots during tests

\- TestDataHelper: Generates large payloads for testing



Extent Reports

Generated Reports include:

\- Test steps and execution flow

\- Info logs

\- Screenshots

\- Pass/fail status



