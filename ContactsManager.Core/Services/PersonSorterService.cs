using System.Linq.Expressions;
using System.Reflection;
using ContactsManager.Core.ServiceContract;
using Entities;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using SerilogTimings;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactsManager.Core.Services
{
    public class PersonSorterService : IPersonSorterService
    {
        private readonly IPersonRepository _personRepository;
        private readonly ILogger<PersonSorterService> _logger;
        //private readonly IDiagnosticContext _diagnosticContext;

        public PersonSorterService(IPersonRepository personRepository, ILogger<PersonSorterService> logger
            /*, IDiagnosticContext diagnosticContext*/)

        {
            _personRepository = personRepository;
            _logger = logger;
            //_diagnosticContext = diagnosticContext;
        }

        /// <summary>
        /// Retrieves persons sorted by a specified property and order.
        /// </summary>
        /// <param name="sortBy">The property to sort by.</param>
        /// <param name="options">The sort order (ascending or descending).</param>
        /// <returns>A list of sorted persons.</returns>
        /// <exception cref="ArgumentException">Thrown when the specified property is not found on the Person class.</exception>
        public async Task<List<PersonResponse>> GetSortedPersonsAsync(string sortBy, SortOrderOptions options)
        {
            List<Person> persons;
            _logger.LogInformation("GetSortedPersonsAsync methods of service PersonService");
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                persons= await _personRepository.GetSortedAscendingAsync(_=>true);
                return persons.Select(p => p.ToPersonResponse()).ToList();
            }

            var property = typeof(Person).GetProperty(sortBy, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            if (property == null)
            {
                throw new ArgumentException($"The sort column '{sortBy}' does not exist on the Person object.", nameof(sortBy));
            }

            var parameter = Expression.Parameter(typeof(Person), "person");
            var propertyAccess = Expression.Property(parameter, property);
            var castToObject = Expression.Convert(propertyAccess, typeof(object));
            var keySelector = Expression.Lambda<Func<Person, object>>(castToObject, parameter);


            using (Operation.Time("Time for GetSortedPersonsAsync execution"))
            {
                persons = options == SortOrderOptions.ASC
                    ? await _personRepository.GetSortedAscendingAsync(keySelector)
                    : await _personRepository.GetSortedDescendingAsync(keySelector);
            }

            return persons.Select(p => p.ToPersonResponse()).ToList();
        }
    }
}
