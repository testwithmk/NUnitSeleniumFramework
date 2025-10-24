using NunitSeleniumFramework.Models.Education;
using OpenQA.Selenium;

namespace NunitSeleniumFramework.Pages.ProfilePages
{
    /**
     * Class for managing Education tab in user profile.
     * Provides methods for adding, editing, deleting, and validating education entries.
     */
    public class EducationPage : BasePage
    {
        private const string tabXpath = "//div[@data-tab='third']";

        //locators
        protected By tabNameLocator = By.XPath("//a[normalize-space()='Education']");
        protected By addNewButton = By.XPath($"{tabXpath}//div[@class='ui teal button ']");
        protected By universityTextbox = By.XPath("//input[@placeholder='College/University Name']");
        protected By countryDropdown = By.XPath("//select[@name='country']");
        protected By titleDropdown = By.XPath("//select[@name='title']");
        protected By degreeTextbox = By.XPath("//input[@placeholder='Degree']");
        protected By yearOfGraduationDropdown = By.XPath("//select[@name='yearOfGraduation']");
        protected By addButton = By.XPath("//input[@value='Add']");
        protected By cancelButton = By.XPath("//input[@value='Cancel']");
        protected By lastCountry = By.XPath($"{tabXpath}//tbody[last()]/tr/td[1]");
        protected By lastUniversity = By.XPath($"{tabXpath}//tbody[last()]/tr/td[2]");
        protected By lastTitle = By.XPath($"{tabXpath}//tbody[last()]/tr/td[3]");
        protected By lastDegree = By.XPath($"{tabXpath}//tbody[last()]/tr/td[4]");
        protected By lastYearOfGraduation = By.XPath($"{tabXpath}//tbody[last()]/tr/td[5]");
        protected By message = By.XPath("//div[@class='ns-box-inner']");
        protected By editButton = By.XPath($"{tabXpath}//tbody[last()]/tr/td[6]/span[1]");
        protected By updateButton = By.XPath("//input[@value='Update']");

        // Track added education entries for cleanup
        private static List<string> addedEducation = new List<string>();

        //constructor
        public EducationPage() { }

        /** Go to Education tab */
        public void GoToTab()
        {
            WaitToBeVisible(tabNameLocator, 5);
            ClickButton(tabNameLocator);
        }

        /** Get tab name */
        public string GetTabName()
        {
            return GetWebDriver().FindElement(tabNameLocator).Text.Trim();
        }

        /**Check if Button is displayed or not
         */
        public bool IsAddNewButtonDisplayed()
        {
            return IsDisplayed(addNewButton);
        }

        /**Add a new Education
         */
        public void AddNewEducation(string university, string country, string title, string degree, string year)
        {
            ClickButton(addNewButton);

            WaitToBeVisible(universityTextbox, 5);
            ClearText(universityTextbox);
            EnterText(universityTextbox, university);

            SelectValueFromDropdown(countryDropdown, country);
            WaitToBeVisible(titleDropdown, 5);
            SelectValueFromDropdown(titleDropdown, title);

            ClearText(degreeTextbox);
            EnterText(degreeTextbox, degree);

            SelectValueFromDropdown(yearOfGraduationDropdown, year);
            ClickButton(addButton);

            // Track added eduaction
            WaitForPageToLoad(2);
            addedEducation.Add(title);
        }

        /** Adds a single education using AddEducation model.
         */
        public string AddEducation(AddEducation edu)
        {
            AddNewEducation(edu.University, edu.Country, edu.Title, edu.Degree, edu.Year);
            return GetMessage();
        }

        /** Adds multiple educations and returns list of messages displayed.
         */
        public List<string> AddMultipleEducation(List<AddEducation> education)
        {
            var messages = new List<string>();
            foreach (var edu in education)
            {
                try
                {
                    AddNewEducation(edu.University, edu.Country, edu.Title, edu.Degree, edu.Year);

                    // Fetch message
                    string msg = GetMessage();
                    messages.Add(msg);

                    if (msg.Contains("already exist") || msg.Contains("Duplicated") || msg.Contains("Please enter") || msg.Contains("invalid"))
                    {
                        ClickButton(cancelButton);
                        WaitForPageToLoad(5);
                    }
                    WaitForPageToLoad(5);

                }
                catch (NoSuchElementException)
                {
                    WaitForPageToLoad(2);
                }
            }
            return messages;
        }

        /**Get text for Education
         */
        public string GetEducation()
        {
            return GetText(lastTitle);
        }

        /**Get text for University
         */
        public string GetUniversity()
        {
            return GetText(lastUniversity);
        }

        /**Get text for Degree
        */
        public string GetDegree()
        {
            return GetText(lastDegree);
        }

        /**Get text for Graduation Year
         */
        public string GetGraduationYear()
        {
            return GetText(lastYearOfGraduation);
        }

        /**Get text for the Message displayed
         */
        public string GetMessage()
        { return GetText(message); }

        /**Edit a education
         */
        public void EditEducation(string editedUniversity, string editedCountry, string editedTitle, string editedDegree, string editedYear)
        {
            ClickButton(editButton);
            WaitToBeVisible(universityTextbox, 5);
            ClearText(universityTextbox);
            EnterText(universityTextbox, editedUniversity);

            SelectValueFromDropdown(countryDropdown, editedCountry);
            WaitToBeVisible(titleDropdown, 5);
            SelectValueFromDropdown(titleDropdown, editedTitle);

            ClearText(degreeTextbox);
            EnterText(degreeTextbox, editedDegree);

            SelectValueFromDropdown(yearOfGraduationDropdown, editedYear);
            WaitToBeClickable(updateButton, 5);
            ClickButton(updateButton);
        }

        /**Delete a education
        */
        public bool DeleteEducation(string expectedEducation)
        {
            try
            {
                var rows = GetWebDriver().FindElements(By.XPath("//div[@data-tab='third']//tbody[last()]/tr"));

                foreach (var row in rows)
                {
                    var presentEducation = row.FindElement(By.XPath("./td[3]")).Text;

                    if (presentEducation == expectedEducation)
                    {
                        //deleted successfully
                        row.FindElement(By.XPath("./td[6]/span[2]")).Click();

                        WaitForPageToLoad(2);
                        return true;
                    }
                }
                //education not found
                return false;
            }
            // no row found
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /**Cleanup method – only delete tracked education
         */
        public void ClearAddedEducationData()
        {
            foreach (var edu in addedEducation)
            {
                try
                {
                    bool deleted = DeleteEducation(edu);
                    if (!deleted)
                    {
                        Console.WriteLine($"'{edu}' was already deleted.");
                        WaitForPageToLoad(2);
                    }
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"'{edu}' is no longer present");
                }
            }

            // Reset list after cleanup
            addedEducation.Clear();
        }

        // check if Education is present or not
        public bool IsEducationPresent(string title)
        {
            try
            {
                var rows = GetWebDriver().FindElements(By.XPath("//div[@data-tab='third']//tbody/tr/td[3]"));
                return rows.Any(r => r.Text.Trim() == title);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

}
