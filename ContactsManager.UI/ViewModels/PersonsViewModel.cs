namespace CrudProject.ViewModels
{
    public class PersonsViewModel
    {
        public int PersonId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public char? Gender { get; set; }
        public int? CountryId { get; set; }
        public string? CountryName { get; set; }
        public bool? ReceiveNewsLetters { get; set; }
        public int? Age { get; set; }
    }
}
