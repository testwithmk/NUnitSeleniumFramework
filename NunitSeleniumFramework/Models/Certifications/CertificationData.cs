namespace NunitSeleniumFramework.Models.Certifications
{
    /**
    * Holds different sets of certification test data (Add, Edit, Duplicate, Invalid) 
    * used for various test scenarios like add, update, and validation.
    */
    public class CertificationData
    {
        public List<AddCertification> Add { get; set; }
        public List<EditCertification> Edit { get; set; }
        public List<AddCertification> Duplicate { get; set; }
        public List<AddCertification> Invalid { get; set; }
    }
}
