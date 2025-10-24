using NunitSeleniumFramework.Models.Education;
using NunitSeleniumFramework.Pages;
using NunitSeleniumFramework.Pages.ProfilePages;
using NunitSeleniumFramework.Utilities;

namespace NunitSeleniumFramework.Tests
{
    /**
    * Contains tests for adding, editing, deleting, and validating educations 
    * on the user's profile, including handling invalid, duplicate, and large payload scenarios.
    */
    [TestFixture]
    public class EducationTest : BaseTest
    {    
        private EducationPage educationPageObj;
        private EducationData eduData;
        private string expectedTabName = "Education";
        private string expectedEducationMessage = "Education has been added";

        [SetUp]
        public void NavigateToEducationTab()
        {
            educationPageObj = new EducationPage();

            // Load JSON for all tests
            eduData = JsonUtil.ReadJsonFile<EducationData>("TestData/education.json");

            var loginPage = new LoginPage();
            loginPage.NavigateToLoginPage();
            loginPage.EnterValidCredentials();
            test.Info("Logged in successfully.");

            educationPageObj.GoToTab();
            Assert.That(educationPageObj.GetTabName, Is.EqualTo(expectedTabName), "Tab name is not correct");

            // Log in ExtentReport
            test.Info("Navigated to Certifications tab successfully.");
        }

        [Test, Description("Add a single education from JSON")]
        public void AddSingleEducation()
        {
            var singleEdu = eduData.Add.First();

            var actualMessage = educationPageObj.AddEducation(singleEdu);
            test.Info($"Adding Education: {singleEdu.Title}");

            Assert.That(actualMessage, Is.EqualTo(expectedEducationMessage), "Education is not added correctly");

            // Capture screenshot after adding
            string screenshotPath = ScreenshotHelper.Capture(GetWebDriver(), "AddSingleEducation");
            test.AddScreenCaptureFromPath(screenshotPath);
        }

        [Test, Description("Add multiple educations from JSON")]
        public void AddMultipleEducations()
        {
            var educations = eduData.Add;

            // Add all educations and get the messages
            var messages = educationPageObj.AddMultipleEducation(educations);
            test.Info("Adding multiple educations.");

            // Verify same number of messages as educations added
            Assert.That(messages.Count, Is.EqualTo(educations.Count),
                $"Expected {educations.Count} messages, but got {messages.Count}");

            // Assert each message
            for (int i = 0; i < educations.Count; i++)
            {
                Assert.That(messages[i], Is.EqualTo(expectedEducationMessage), "Education is not added correctly");
            }
            test.Pass("Multiple educations added successfully.");
        }

        [Test, Description("Add a single education from JSON and then edit that education")]
        public void EditEducation()
        {
            AddSingleEducation();

            var editEdu = eduData.Edit.First();

            educationPageObj.EditEducation(editEdu.UpdatedUniversity, editEdu.UpdatedCountry, editEdu.UpdatedTitle, editEdu.UpdatedDegree, editEdu.UpdatedYear);
            test.Info($"Editing education to: {editEdu.UpdatedTitle}");

            Assert.That(educationPageObj.GetMessage(), Is.EqualTo("Education as been updated"), "Education is not updated.");
            Assert.That(educationPageObj.GetEducation(), Is.EqualTo(editEdu.UpdatedTitle), "Title is not updated.");
            Assert.That(educationPageObj.GetUniversity(), Is.EqualTo(editEdu.UpdatedUniversity), "University is not updated.");
            Assert.That(educationPageObj.GetDegree(), Is.EqualTo(editEdu.UpdatedDegree), "Degree is not updated.");
            Assert.That(educationPageObj.GetGraduationYear(), Is.EqualTo(editEdu.UpdatedYear), "Year is not updated.");

            test.Pass("Education edited successfully.");
        }

        [Test, Description("Add a single education from JSON and then delete that education")]
        public void DeleteEducation()
        {
            AddSingleEducation();
            var educationToDelete = eduData.Add.First().Title;

            educationPageObj.DeleteEducation(educationToDelete);
            test.Info($"Deleting education: {educationToDelete}");

            Assert.That(educationPageObj.GetMessage(), Is.EqualTo("Education entry successfully removed"), "Education is not deleted.");
            test.Pass("Education deleted successfully.");
        }

        [Test, Description("Verify that textfields can handle large payload for destructive testing")]
        public void VerifyEducationWithLargePayload()
        {
            string largePayload = TestDataHelper.GenerateLargePayload(300, 400, 200, 300);
            test.Info("Generated large payload for destructive testing.");

            educationPageObj.AddNewEducation(largePayload, "Austria", "B.Sc", largePayload, "2018");
            test.Info("Added education with large payload.");

            Assert.That(educationPageObj.GetMessage(), Is.EqualTo(expectedEducationMessage), "Education is not added correctly.");
        }

        [Test, Description("Verify duplicate error messages when adding same education")]
        public void VerifyDuplicateEducationMessages()
        {

            var duplicateEdu = eduData.Duplicate;

            // Expected messages
            var expectedMessages = new List<string>
        {
            $"{expectedEducationMessage}",
            "This information is already exist.",
            "Duplicated data"
        };

            // Add all educations at once and get messages
            var actualMessages = educationPageObj.AddMultipleEducation(duplicateEdu);
            test.Info("Adding duplicate educations to check error messages.");

            // Assert all messages
            Assert.That(actualMessages, Is.EqualTo(expectedMessages), "Duplicate education messages are not correct");
            test.Pass("Duplicate education messages verified successfully.");
        }

        [Test, Description("Verify messages while adding invalid input to education")]
        public void VerifyInvalidEducationMessages()
        {

            var invalidEdu = eduData.Invalid;

            // Expected messages
            var expectedMessages = new List<string>
        { 
            "Education information was invalid",
            $"{expectedEducationMessage}",
            $"{expectedEducationMessage}",
            "Please enter all the fields"
        };

            // Add all educations at once and get messages
            var actualMessages = educationPageObj.AddMultipleEducation(invalidEdu);
            test.Info("Adding invalid educations to check error messages.");

            // Assert all messages 
            Assert.That(actualMessages, Is.EqualTo(expectedMessages), "Invalid education messages are not correct");
            test.Pass("Invalid education messages verified successfully.");
        }

        [TearDown]
        public void Cleanup()
        {
            if (educationPageObj != null)
                educationPageObj.ClearAddedEducationData();
            test.Info("Cleanup completed for added certifications.");
        }
    }
}
