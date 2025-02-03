using Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    /// <summary>
    ///  Represents the DTO class that contains the person details to update
    /// </summary>
    public class PersonUpdateRequest
    {
        [Required]
        public int PersonId { get; set; }
        [DisplayName("FirstName")]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public char? Gender { get; set; }
        public int? CountryId { get; set; }
        public bool ReceiveNewsLetters { get; set; }

        public Person ToPerson() => new Person()
        {
            PersonId=this.PersonId,
            FirstName = this.FirstName,
            LastName = this.LastName,
            DateOfBirth = this.DateOfBirth,
            Gender = this.Gender,
            CountryId = this.CountryId,
            ReceiveNewsLetters = this.ReceiveNewsLetters,
        };
    }
}