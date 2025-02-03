using Entities;
using System.ComponentModel.DataAnnotations;


namespace ServiceContracts.DTO
{
    public class PersonAddRequest
    {
        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }        

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime? DateOfBirth { get; set; }

        [Required]
     // [AllowedValues("M", "F")]
        public char? Gender { get; set; }

        public int? CountryId { get; set; }
   
        public bool ReceiveNewsLetters { get; set; }
    }

    public static class PersonExtensions
    {
        public static Person ToPerson(this PersonAddRequest personAddRequest) =>
           new Person
           {
               FirstName = personAddRequest.FirstName,
               LastName = personAddRequest.LastName,
               CountryId = personAddRequest.CountryId,
               DateOfBirth = personAddRequest.DateOfBirth,
               Email = personAddRequest.Email,
               Gender = personAddRequest.Gender,
               ReceiveNewsLetters = personAddRequest.ReceiveNewsLetters,
               

           };
        public static PersonResponse ToPersonResponse(this Person person) =>
            new PersonResponse
            {
                CountryId = person.CountryId,
                DateOfBirth = person.DateOfBirth,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Email = person.Email,
                Gender = person.Gender,
                PersonId = person.PersonId,
                ReceiveNewsLetters
                = person.ReceiveNewsLetters,
                CountryName=person.Country?.CountryName

            };

        public static PersonResponse ToPersonResponse(this PersonAddRequest personAddRequest) =>
            new PersonResponse
            {
                CountryId = personAddRequest.CountryId,
                DateOfBirth = personAddRequest.DateOfBirth,
                FirstName = personAddRequest.FirstName,
                LastName = personAddRequest.LastName,
                Email = personAddRequest.Email,
                Gender = personAddRequest.Gender,
                ReceiveNewsLetters
                = personAddRequest.ReceiveNewsLetters
            };
    }
}
