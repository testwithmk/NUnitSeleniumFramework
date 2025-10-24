namespace NunitSeleniumFramework.Models.Education
{
    /**
    * Represents the data model for adding an education record 
    * on the user's profile during automated tests.
    */
    public class AddEducation
    {
        public string University { get; set; }
        public string Country { get; set; }
        public string Title { get; set; }
        public string Degree { get; set; }
        public string Year { get; set; }
    }
}
