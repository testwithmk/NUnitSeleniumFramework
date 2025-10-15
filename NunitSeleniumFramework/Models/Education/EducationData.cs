namespace NunitSeleniumFramework.Models.Education
{
    /**
    * Represents the collection of education test data used for automated tests.
    * Includes lists for adding, editing, duplicate, and invalid education records.
    */
    public class EducationData
    {
        public List<AddEducation> Add { get; set; }
        public List<EditEducation> Edit { get; set; }
        public List<AddEducation> Duplicate { get; set; }
        public List<AddEducation> Invalid { get; set; }
    }
}
