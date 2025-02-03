using ContactsManager.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Core.Services
{
    public class PersonUpdaterService : IPersonUpdaterService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonUpdaterService> _logger;
        //private readonly IDiagnosticContext _diagnosticContext;

        public PersonUpdaterService(IPersonRepository personRepository, ILogger<PersonUpdaterService> logger
            /*, IDiagnosticContext diagnosticContext*/)

        {
            _personRepository = personRepository;
            _logger = logger;
            //_diagnosticContext = diagnosticContext;
        }

    
        /// <summary>   
        /// Updates an existing person's details.
        /// </summary>
        /// <param name="id">The ID of the person to update.</param>
        /// <param name="personUpdateRequest">The updated details of the person.</param>
        /// <returns>A response object representing the updated person.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the update request is null.</exception>
        /// <exception cref="ArgumentException">Thrown when the specified person ID does not exist in the repository.</exception>
        public async Task<PersonResponse> UpdatePersonAsync(int id, PersonUpdateRequest? personUpdateRequest)
        {
            if (personUpdateRequest == null)
                throw new ArgumentNullException(nameof(personUpdateRequest), "Person to update cannot be null");

            if (!await _personRepository.ExistsAsync(id))
                throw new ArgumentException($"No person with id {personUpdateRequest.PersonId}");

            var updatedPerson = await _personRepository.UpdatePersonAsync(id, personUpdateRequest.ToPerson());
            return updatedPerson.ToPersonResponse();
        }
    }
}
