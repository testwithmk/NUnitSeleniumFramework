using NunitSeleniumFramework.Models;
using OpenQA.Selenium;

namespace NunitSeleniumFramework.Pages.ProfilePages
{
    /**
     * Class for managing Certifications tab in user profile.
     * Provides methods for adding, editing, deleting, and validating certifications.
     */
    public class CertificationsPage : BasePage
    {
        private const string tabXpath = "//div[@data-tab='fourth']";

        //locators
        protected By tabNameLocator = By.XPath("//a[normalize-space()='Certifications']");
        protected By addNewButton = By.XPath($"{tabXpath}//div[@class='ui teal button ']");
        protected By certificateTextbox = By.XPath("//input[@placeholder='Certificate or Award']");
        protected By certifiedFromTextbox = By.XPath("//input[@placeholder='Certified From (e.g. Adobe)']");
        protected By certificationYearDropdown = By.XPath("//select[@name='certificationYear']");
        protected By addButton = By.XPath("//input[@value='Add']");
        protected By cancelButton = By.XPath("//input[@value='Cancel']");
        protected By lastCertificate = By.XPath($"{tabXpath}//tbody[last()]/tr/td[1]");
        protected By lastCertifiedFrom = By.XPath($"{tabXpath}//tbody[last()]/tr/td[2]");
        protected By lastCertificationYear = By.XPath($"{tabXpath}//tbody[last()]/tr/td[3]");
        protected By message = By.XPath("//div[@class='ns-box-inner']");
        protected By editButton = By.XPath($"{tabXpath}//tbody[last()]/tr/td[4]/span[1]");
        protected By updateButton = By.XPath("//input[@value='Update']");

        // Track added certifications to clean up later
        private static List<string> addedCertifications = new List<string>();


        //constructor
        public CertificationsPage() { }

        /** Go to Certification tab */
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

        /**Add a new certificate
         */
        public void AddNewCertification(string certificate, string certificateFrom, string Year)
        {
            ClickButton(addNewButton);

            WaitToBeVisible(certificateTextbox, 5);
            ClearText(certificateTextbox);
            EnterText(certificateTextbox, certificate);
            ClearText(certifiedFromTextbox);
            EnterText(certifiedFromTextbox, certificateFrom);

            SelectValueFromDropdown(certificationYearDropdown, Year);
            ClickButton(addButton);

            // Track added certification
            WaitForPageToLoad(2);
            addedCertifications.Add(certificate);
        }

        /** Adds a single certification using AddCertification model.
         */
        public string AddCertification(AddCertification cert)
        {
            AddNewCertification(cert.Certificate, cert.CertifiedFrom, cert.Year);
            return GetMessage();
        }

        /** Adds multiple certifications and returns list of messages displayed. 
         */
        public List<string> AddMultipleCertifications(List <AddCertification> certifications)
        {
            var messages = new List<string>();
            foreach (var cert in certifications)
            {
                try
                {
                    AddNewCertification(cert.Certificate, cert.CertifiedFrom, cert.Year);

                    // Fetch message
                    string msg = GetMessage();
                    messages.Add(msg);

                    if (msg.Contains("already exist") || msg.Contains("Duplicated") || msg.Contains("Please enter"))
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

        /**Get text for Certificate
         */
        public string GetCertificate()
        {
            return GetText(lastCertificate);
        }

        /**Get text for Certified From
         */
        public string GetCertifiedFrom()
        {
            return GetText(lastCertifiedFrom);
        }

        /**Get text for Certification Year
         */
        public string GetCertificationYear()
        {
            return GetText(lastCertificationYear);
        }

        /** Get any message displayed on page.
         */
        public string GetMessage()
        { return GetText(message); }

        /**Edit a certificate
         */
        public void EditCertificate(string editedCertificate, string editedCertifiedFrom, string editedCertificationYear)
        {
            ClickButton(editButton);
            ClearText(certificateTextbox);
            EnterText(certificateTextbox, editedCertificate);
            ClearText(certifiedFromTextbox);
            EnterText(certifiedFromTextbox, editedCertifiedFrom);
            SelectValueFromDropdown(certificationYearDropdown, editedCertificationYear);
            WaitToBeClickable(updateButton, 8);
            ClickButton(updateButton);
        }

        /**Delete a certificate
        */
        public bool DeleteCertificate(string expectedCertificate)
        {
            try
            {
                var rows = GetWebDriver().FindElements(By.XPath("//div[@data-tab='fourth']//tbody[last()]/tr"));

                foreach (var row in rows)
                {
                    var presentCertificate = row.FindElement(By.XPath("./td[1]")).Text;

                    if (presentCertificate == expectedCertificate)
                    {
                        //deleted successfully
                        row.FindElement(By.XPath("./td[4]/span[2]")).Click();

                        WaitForPageToLoad(2);
                        return true;
                    }
                }
                //language not found
                return false;
            }
            // no row found
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        /**Cleanup method – only delete tracked certificates
         */
        public void ClearAddedCertificatesData()
        {
            foreach (var cert in addedCertifications)
            {
                try
                {
                    bool deleted = DeleteCertificate(cert);
                    if (!deleted)
                    {
                        Console.WriteLine($"'{cert}' was already deleted.");
                        WaitForPageToLoad(2);
                    }
                }
                catch (StaleElementReferenceException)
                {
                    Console.WriteLine($"'{cert}' is no longer present");
                }
            }

            // Reset list after cleanup
            addedCertifications.Clear();
        }

        // check if certificate is present or not
        public bool IsCertificatePresent(string certificate)
        {
            try
            {
                var rows = GetWebDriver().FindElements(By.XPath("//div[@data-tab='fourth']//tbody/tr/td[1]"));
                return rows.Any(r => r.Text.Trim() == certificate);
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }

}
