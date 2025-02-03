namespace Entities
{
    /// <summary>
    /// Domain model for storing the country details 
    /// </summary>
    public class Country
    {

        // A GUID was used in the video because 
        // int int big int has a limited a range of numbers 
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        //public List<Person> Persons { get; set; }
    }
}
