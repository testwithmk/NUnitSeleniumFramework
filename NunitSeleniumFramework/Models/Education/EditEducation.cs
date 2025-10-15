namespace NunitSeleniumFramework.Models.Education
{
    /**
    * Represents the data model for editing an existing education record 
    * on the user's profile during automated tests.
    */
    public class EditEducation
    {
        public string UpdatedUniversity { get; set; }
        public string UpdatedCountry { get; set; }
        public string UpdatedTitle { get; set; }
        public string UpdatedDegree { get; set; }
        public string UpdatedYear { get; set; }
    }
}
