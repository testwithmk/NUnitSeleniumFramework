namespace NunitSeleniumFramework.Models.Certifications
{
    /**
    * Represents the data model for editing an existing certification record 
    * on the user's profile during automated tests.
    */
    public class EditCertification
    {
        public string UpdatedCertificate { get; set; }
        public string UpdatedCertifiedFrom { get; set; }
        public string UpdatedYear { get; set; }
    }
}
