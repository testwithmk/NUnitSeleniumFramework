using NunitSeleniumFramework.Utilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace NunitSeleniumFramework.Pages
{
    /**
    * Serves as the base class for all page objects.
    * Provides common Selenium WebDriver actions and utilities 
    * such as clicking, entering text, waiting for elements, and selecting from dropdowns.
    */
    public class BasePage : CommonDriver
    {
        /** Clicks on an element
        */
        public static void ClickButton(By locator)
        {
            WaitToBeClickable(locator, 5);
            GetWebDriver().FindElement(locator).Click();
        }

        /**Enters text into an input field
         */
        public static void EnterText(By locator, string text)
        {
            WaitToBeVisible(locator, 5);
            var element = GetWebDriver().FindElement(locator);
            element.Clear();

            // Handle single space
            if (text == " ")
            {
                element.SendKeys(Keys.Space);
            }
            else
            {
                element.SendKeys(text);
            }
        }

        /**Returns the visible text of an element
         */
        public static string GetText(By locator)
        {
            WaitToBeVisible(locator, 5);
            return GetWebDriver().FindElement(locator).Text;
        }

        /**Returns the 'value' attribute of an input element.
        */
        public static string GetValue(By locator)
        {
            WaitToBeVisible(locator, 5);
            return GetWebDriver().FindElement(locator).GetAttribute("value");
        }

        /** Returns the list of values from elements matching the locator */
        public static List<string> GetListOfValues(By locator)
        {
            WaitToBeVisible(locator, 5);
            var elements = GetWebDriver().FindElements(locator);
            return elements.Select(e => e.GetAttribute("value")).ToList();
        }

        /**Checks if an element is displayed.
        */
        public static bool IsDisplayed(By locator)
        {
            try
            {
                return GetWebDriver().FindElement(locator).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /** Waits for the entire page to be fully loaded 
          * @param timeoutSeconds: to give time in seconds
          */
        public static void WaitForPageToLoad(int timeoutSeconds = 10)
        {

            var wait = new WebDriverWait(GetWebDriver(), TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(d =>
            ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").ToString() == "complete");
        }

        /** Waits until the specified element becomes clickable. 
         *  @param locatorValue: locator of the element to wait
         *  @param seconds : time duration in seconds
         */
        public static void WaitToBeClickable(By locatorValue, int seconds)
        {
            var wait = new WebDriverWait(GetWebDriver(), TimeSpan.FromSeconds(seconds));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locatorValue));

        }

        /** Waits until the specified element becomes visible on the page. 
        *  @param locatorValue: locator of the element to wait
        *  @param seconds : time duration in seconds
        */
        public static void WaitToBeVisible(By locatorValue, int seconds)
        {
            var wait = new WebDriverWait(GetWebDriver(), TimeSpan.FromSeconds(seconds));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locatorValue));
        }

        /**Select the value from a dropdown
        */
        public static void SelectValueFromDropdown(By locator, string value)
        {
            var element = GetWebDriver().FindElement(locator);
            var selectElement = new SelectElement(element);
            selectElement.SelectByValue(value);
        }

        /**Clear the text of an element
         */
        public void ClearText(By locator)
        {
            WaitToBeVisible(locator, 5);
            GetWebDriver().FindElement(locator).Clear();
        }
    }
}
