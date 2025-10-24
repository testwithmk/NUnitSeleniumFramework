using NunitSeleniumFramework.Models.Certifications;
using NunitSeleniumFramework.Pages;
using NunitSeleniumFramework.Pages.ProfilePages;
using NunitSeleniumFramework.Utilities;


namespace NunitSeleniumFramework.Tests
{
    /**
    * Contains tests for adding, editing, deleting, and validating certifications 
    * on the user's profile, including handling invalid, duplicate, and large payload scenarios.
    */
    [TestFixture]
    public class CertificationsTest : BaseTest
    {
        private string expectedTabName = "Certifications";
        private CertificationsPage certificationPageObj;
        private CertificationData certData;


        [SetUp]
        public void NavigateToCertificationTab()
        {
            certificationPageObj = new CertificationsPage();

            // Load JSON for all tests
            certData = JsonUtil.ReadJsonFile<CertificationData>("TestData/certifications.json");

            var loginPage = new LoginPage();
            loginPage.NavigateToLoginPage();
            loginPage.EnterValidCredentials();
            test.Info("Logged in successfully.");

            certificationPageObj.GoToTab();
            Assert.That(certificationPageObj.GetTabName, Is.EqualTo(expectedTabName), "Tab name is not correct");

            // Log in ExtentReport
            test.Info("Navigated to Certifications tab successfully.");
        }

        [Test, Description("Add a single certification from JSON")]
        public void AddSingleCertification()
        {
            var singleCert = certData.Add.First();

            var actualMessage = certificationPageObj.AddCertification(singleCert);
            test.Info($"Adding certification: {singleCert.Certificate}");

            Assert.That(actualMessage, Is.EqualTo($"{singleCert.Certificate} has been added to your certification"), "Certification is not added correctly");

            // Capture screenshot after adding
            string screenshotPath = ScreenshotHelper.Capture(GetWebDriver(), "AddSingleCertification");
            test.AddScreenCaptureFromPath(screenshotPath);
        }

        [Test, Description("Add multiple certifications from JSON")]
        public void AddMultipleCertifications()
        {
            var certifications = certData.Add;

            // Add all certifications and get the messages
            var messages = certificationPageObj.AddMultipleCertifications(certifications);
            test.Info("Adding multiple certifications.");

            // Verify same number of messages as certifications added
            Assert.That(messages.Count, Is.EqualTo(certifications.Count), $"Expected {certifications.Count} messages, but got {messages.Count}");

            // Assert each message
            for (int i = 0; i < certifications.Count; i++)
            {
                string expectedMessage = $"{certifications[i].Certificate} has been added to your certification";
                Assert.That(messages[i], Is.EqualTo(expectedMessage), $"Certification '{certifications[i].Certificate}' is not added correctly");
            }
        }

        [Test, Description("Add a single certification from JSON and then edit that certificate")]
        public void EditCertification()
        {
            AddSingleCertification();

            var editCert = certData.Edit.First();

            certificationPageObj.EditCertificate(editCert.UpdatedCertificate, editCert.UpdatedCertifiedFrom, editCert.UpdatedYear);
            test.Info($"Editing certification to: {editCert.UpdatedCertificate}");

            Assert.That(certificationPageObj.GetMessage(), Is.EqualTo($"{editCert.UpdatedCertificate} has been updated to your certification"), "Certificate is not updated.");
            Assert.That(certificationPageObj.GetCertifiedFrom(), Is.EqualTo(editCert.UpdatedCertifiedFrom), "Certified From is not updated.");
            Assert.That(certificationPageObj.GetCertificationYear(), Is.EqualTo(editCert.UpdatedYear), "Year is not updated.");

            string screenshotPath = ScreenshotHelper.Capture(GetWebDriver(), "EditCertification");
            test.AddScreenCaptureFromPath(screenshotPath);
        }

        [Test, Description("Add a single certification from JSON and then delete that certificate")]
        public void DeleteCertification()
        {
            AddSingleCertification();
            var certificateToDelete = certData.Add.First().Certificate;

            certificationPageObj.DeleteCertificate(certificateToDelete);
            test.Info($"Deleting certification: {certificateToDelete}");

            Assert.That(certificationPageObj.GetMessage(), Is.EqualTo($"{certificateToDelete} has been deleted from your certification"), "Certificate is not deleted.");

            string screenshotPath = ScreenshotHelper.Capture(GetWebDriver(), "DeleteCertification");
            test.AddScreenCaptureFromPath(screenshotPath);
        }

        [Test, Description("Verify that certification field can handle large payload for destructive testing")]
        public void VerifyCertificationWithLargePayload()
        {
            string largePayload = TestDataHelper.GenerateLargePayload(300, 400, 200, 300);
            test.Info("Generated large payload for destructive testing.");

            certificationPageObj.AddNewCertification(largePayload, largePayload, "2016");
            test.Info("Added certification with large payload.");

            Assert.That(certificationPageObj.GetMessage().Contains("9999999"), Is.True, "Message does not contain expected payload part");
        }

        [Test, Description("Verify duplicate error messages when adding same certifications")]
        public void VerifyDuplicateCertificationMessages()
        {

            var duplicateCerts = certData.Duplicate;

            // Expected messages
            var expectedMessages = new List<string>
        {
            "Platform has been added to your certification",
            "This information is already exist.",
            "Duplicated data"
        };

            var actualMessages = certificationPageObj.AddMultipleCertifications(duplicateCerts);
            test.Info("Adding duplicate certifications to check error messages.");

            // Assert all messages
            Assert.That(actualMessages, Is.EqualTo(expectedMessages), "Duplicate certification messages are not correct");
        }

        [Test, Description("Verify messages while adding invalid input to certification")]
        public void VerifyInvalidCertificationMessages()
        {

            var invalidCerts = certData.Invalid;

            // Expected messages
            var expectedMessages = new List<string>
        {
            "Please enter Certification Name, Certification From and Certification Year",
            "has been added to your certification",
            "%!^&*()_+&#58 has been added to your certification",
             "<script> has been added to your certification", 
        };

            var actualMessages = certificationPageObj.AddMultipleCertifications(invalidCerts);
            test.Info("Adding invalid certifications to check error messages.");

            // Assert all messages 
            Assert.That(actualMessages, Is.EqualTo(expectedMessages), "Invalid certification messages are not correct");
        }

        [TearDown]
        public void Cleanup()
        {
            if (certificationPageObj != null)
                certificationPageObj.ClearAddedCertificatesData();
            test.Info("Cleanup completed for added certifications.");
        }
    }
}
