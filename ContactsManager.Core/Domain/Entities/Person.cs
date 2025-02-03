namespace Entities
{
    /// <summary>
    /// Person domain model class 
    /// </summary>
    public class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public char? Gender { get; set; }
        public int? CountryId { get; set; }
        public Country? Country { get; set; }
        public bool ReceiveNewsLetters { get; set; }

    }
}
