using ContactsManager.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace ContactsManager.Core.Services
{
    public class PersonDeleterService : IPersonDeleterService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonDeleterService> _logger;
        //private readonly IDiagnosticContext _diagnosticContext;

        public PersonDeleterService(IPersonRepository personRepository, ILogger<PersonDeleterService> logger
            /*, IDiagnosticContext diagnosticContext*/)

        {
            _personRepository = personRepository;
            _logger = logger;
            //_diagnosticContext = diagnosticContext;
        }

        /// <summary>
        /// Deletes a person from the repository by their ID.
        /// </summary>
        /// <param name="personId">The ID of the person to delete.</param>
        /// <returns>True if the person was deleted; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the person ID is null.</exception>
        public async Task<bool> DeletePersonAsync(int? personId)
        {
            if (personId == null)
                throw new ArgumentNullException(nameof(personId.Value));

            var doesPersonExist = await _personRepository.ExistsAsync(personId.Value);
            if (!doesPersonExist)
                return false;

            return await _personRepository.DeletePersonAsync(personId.Value);
        }

       
    }
}
