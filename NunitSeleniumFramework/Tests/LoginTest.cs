using NunitSeleniumFramework.Pages;
using NunitSeleniumFramework.Utilities;

namespace NunitSeleniumFramework.Tests
{
    /** 
     * Performs login with valid credentials.
     */
    [TestFixture]
    public class LoginTest : BaseTest
    {
        private LoginPage loginPageObj;

        // method to be called from other tests
        public void PerformLogin()
        {
            loginPageObj = new LoginPage();
            loginPageObj.NavigateToLoginPage();
            test.Info("Navigated to Login Page.");

            loginPageObj.EnterValidCredentials();
            test.Info("Entered valid credentials and submitted login form.");
        }

        [Test, Description("This test verifies successful Login to Project Mars")]
        public void LoginTestMethod()
        {
            PerformLogin();

            string actualWelcomeText = loginPageObj.GetWelcomeText();
            string expectedWelcomeText = "Hi " + ConfigReader.GetValue("User_FirstName");
            test.Info($"Expected Welcome Text: {expectedWelcomeText}");
            test.Info($"Actual Welcome Text: {actualWelcomeText}");

            Assert.That(actualWelcomeText, Is.EqualTo(expectedWelcomeText), "User did not navigate to Profile page.");
            test.Pass("Login test passed successfully.");

        }        
    }
}
