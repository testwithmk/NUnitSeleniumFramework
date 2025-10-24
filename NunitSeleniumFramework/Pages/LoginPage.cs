using NunitSeleniumFramework.Utilities;
using OpenQA.Selenium;

namespace NunitSeleniumFramework.Pages
{
    /**
    * Represents the login page of the application.
    * Provides methods to navigate to the login page, 
    * enter user credentials, and perform login actions.
    */
    public class LoginPage : BasePage
    {
        //Locators
        private By signInButton = By.XPath("//a[contains(text(),'Sign In')]");
        private By emailTextBox = By.Name("email");
        private By passwordTextbox = By.Name("password");
        private By loginButton = By.XPath("//*[contains(text(),'Login')]");
        private By welcomeText = By.XPath("//span[@class='item ui dropdown link ']");

        private string url = ConfigReader.GetValue("Url");

        public LoginPage()
        { }

        /**Navigate to Login Page
         */
        public void NavigateToLoginPage()
        {
            // Launch the Mars portal
            GetWebDriver().Navigate().GoToUrl(url);
            WaitToBeClickable(signInButton, 5);
            try
            {
                ClickButton(signInButton);

            }
            catch (NoSuchElementException)
            {
                Assert.Fail("Sign In button was not located.");
            }
        }

        /**Enter user credentials to login
        */
        public void EnterCredentials(string email, string password)
        {
            WaitToBeVisible(emailTextBox, 5);
            EnterText(emailTextBox, email);
            EnterText(passwordTextbox, password);
        }

        /**Click on login button
       */
        public void ClickLoginButton()
        {
            ClickButton(loginButton);
        }

        /**Fetch welcome message for user
       */
        public string GetWelcomeText()
        {
            WaitToBeVisible(welcomeText, 5);
            return GetText(welcomeText);
        }

        /**Enter valid user credentials to login from config file
        */
        public void EnterValidCredentials()
        {
            string email = ConfigReader.GetValue("Email");
            string password = ConfigReader.GetValue("Password");
            EnterCredentials(email, password);
            ClickLoginButton();
        }

    }
}
