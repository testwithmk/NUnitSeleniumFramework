namespace NunitSeleniumFramework.Models
{
    /**
	* Represents the data model for adding a certification record on the user's profile. 
	* This class is used to map JSON test data and pass certification details to page actions during automated tests.
	*/
    public class AddCertification
    {
		public string Certificate { get; set; }
		public string CertifiedFrom { get; set; }
		public string Year { get; set; }

	}

}
