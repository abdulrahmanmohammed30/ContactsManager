using System.Reflection;
using ContactsManager.Core.ServiceContract;
using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using SerilogTimings;
using ServiceContracts.DTO;

namespace ContactsManager.Core.Services
{
    public class PersonGetterService : IPersonGetterService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonGetterService> _logger;
        //private readonly IDiagnosticContext _diagnosticContext;

        public PersonGetterService(IPersonRepository personRepository, ILogger<PersonGetterService> logger
            /*, IDiagnosticContext diagnosticContext*/)

        {
            _personRepository = personRepository;
            _logger = logger;
            //_diagnosticContext = diagnosticContext;
        }


        /// <summary>
        /// Retrieves all persons from the repository.
        /// </summary>
        /// <returns>A list of all persons.</returns>
        public async Task<List<PersonResponse>> GetAllPersonsAsync()
        {
            _logger.LogInformation("GetAllPersonsAsync action method of PersonService");
            var persons = await _personRepository.GetAllPersonsAsync();
            //_diagnosticContext.Set("persons", persons.Select(p => $"Name: {p.FirstName + " " + p.LastName}\t Id: {p.PersonId}\t Email: {p.Email}"));

            return persons.Select(p => p.ToPersonResponse()).ToList();
        }

        /// <summary>
        /// Retrieves persons filtered by a specified property and value.
        /// </summary>
        /// <param name="searchBy">The property to search by (e.g., "firstname").</param>
        /// <param name="searchString">The value to search for.</param>
        /// <exception cref="ArgumentException">Thrown when the specified property is not found on the Person class.</exception>
        /// <returns>A list of filtered persons.</returns>
        public async Task<List<PersonResponse>> GetFilteredPersonsAsync(string searchBy, string? searchString)
        {
            List<Person> results;
            using (Operation.Time("Time for Filtered Persons from Database"))
            {
                // Think before 
                // What are all the possible cases? 
                // searchBy, searchString
                // Note: consider empty & null to be equivalent  
                // null null, null value --> return all the elements 
                // value null -> have to check first if the property exists or not cause if It property did not exists
                // and method returned all the elements, user will be confused, thinking the property exists 
                // value value -> normal case, do the filtering 
                // There for the following condition is wrong
                /*
                if (string.IsNullOrWhiteSpace(searchBy) || string.IsNullOrWhiteSpace(searchString))
                */
                // You got to ensure first that the searchBy is not or not 
                // if it was null then the special case if returning all data applies 
                // otherwise, you have to go and find out if the property exists on the type or not 
                // if it doesn't not exist, throw an exception 
                // if it does exists, then we will be looking at the value 
                // avoid using reflection 
                if (string.IsNullOrWhiteSpace(searchBy))
                {
                    var persons = await _personRepository.GetAllPersonsAsync();
                    return persons.Select(p => p.ToPersonResponse()).ToList();
                }

                var propertyToSearchBy = typeof(PersonResponse).GetProperty(searchBy,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyToSearchBy == null)
                {
                    throw new ArgumentException($"{searchBy} is not a valid property name.");
                }

                if (string.IsNullOrWhiteSpace(searchString))
                {
                    var persons = await _personRepository.GetFilteredPersonsAsync(p=>p.FirstName.Contains(""));
                    return persons.Select(p => p.ToPersonResponse()).ToList();
                }

                results = searchBy switch
                {
                    nameof(PersonResponse.FirstName) => await _personRepository.GetFilteredPersonsAsync(p =>
                        p.FirstName.ToLower().Contains(searchString.ToLower())),
                    nameof(PersonResponse.LastName) => await _personRepository.GetFilteredPersonsAsync(p =>
                        p.LastName.ToLower().Contains(searchString.ToLower())),
                    nameof(PersonResponse.Email) => await _personRepository.GetFilteredPersonsAsync(p =>
                        p.Email.ToLower().Contains(searchString.ToLower())),
                    nameof(PersonResponse.CountryId) when int.TryParse(searchString, out var countryId) =>
                        await _personRepository.GetFilteredPersonsAsync(p => p.CountryId == countryId),
                    nameof(PersonResponse.ReceiveNewsLetters) when
                        bool.TryParse(searchString, out var receiveNewsLetters) => await _personRepository
                            .GetFilteredPersonsAsync(p => p.ReceiveNewsLetters == receiveNewsLetters),
                    _ => throw new InvalidOperationException($"Cannot filter by property {searchBy}")
                };

            }

            return results.Select(p => p.ToPersonResponse()).ToList();
        }

        /// <summary>
        /// Retrieves a person by their ID.
        /// </summary>
        /// <param name="id">The ID of the person to retrieve.</param>
        /// <returns>A response object for the person, or null if not found.</returns>
        public async Task<PersonResponse?> GetPersonByPersonIdAsync(int? id)
        {
            if (id == null)
                return null;

            Person? person = null;
            using (Operation.Time("Time for GetPersonByPersonIdAsync execution"))
            {
                person = await _personRepository.GetPersonByPersonIdAsync(id.Value);
            }

            return person?.ToPersonResponse();
        }
        
        public async Task<bool> Exists(int id)
        {
            return await _personRepository.ExistsAsync(id);
        }
    }
}
