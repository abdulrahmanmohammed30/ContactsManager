using System.Text.RegularExpressions;
using ContactsManager.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Core.Services
{
    public class PersonAdderService : IPersonAdderService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonAdderService> _logger;
        //private readonly IDiagnosticContext _diagnosticContext;

        public PersonAdderService(IPersonRepository personRepository, ILogger<PersonAdderService> logger
            /*, IDiagnosticContext diagnosticContext*/)

        {
            _personRepository = personRepository;
            _logger = logger;
            //_diagnosticContext = diagnosticContext;
        }

        /// <summary>
        /// Adds a new person to the repository.
        /// </summary>
        /// <param name="personAddRequest">The details of the person to add.</param>
        /// <returns>A response object representing the added person.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the request object is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the first name is null or empty.</exception>
        public async Task<PersonResponse> AddPersonAsync(PersonAddRequest? personAddRequest)
        {
            if (personAddRequest == null)
                throw new ArgumentNullException(nameof(personAddRequest));

            if (string.IsNullOrWhiteSpace(personAddRequest.FirstName))
                throw new ArgumentException("Person firstname cannot be null");

            if (string.IsNullOrWhiteSpace(personAddRequest.LastName))
                throw new ArgumentException("Person firstname cannot be null");

            if (Regex.IsMatch(personAddRequest.Email, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-zA-Z](([\-\.\w]*[0-9a-zA-Z])*)\.)+[a-zA-Z]{2,}))$"))
                throw new ArgumentException("Email address is invalid");

            var addedPerson = await _personRepository.AddPersonAsync(personAddRequest.ToPerson());
            return addedPerson.ToPersonResponse();
        }



 
    }
}
