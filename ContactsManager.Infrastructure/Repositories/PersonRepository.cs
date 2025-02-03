using System.Linq.Expressions;
using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace ContactsManager.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PersonRepository> _logger;
        public PersonRepository(AppDbContext context, ILogger<PersonRepository> logger)
        {   
            _context = context;
            _logger = logger;
        }

        public async Task<Person> AddPersonAsync(Person person)
        {
            await _context.Persons.AddAsync(person);
            await _context.SaveChangesAsync();
            return person;
        }

        public async Task<bool> DeletePersonAsync(int personId)
        {
            var person = new Person { PersonId = personId };

            try
            {
                _context.Persons.Remove(person);
                int rowsDeleted =  await _context.SaveChangesAsync();
                return rowsDeleted > 0; 
            }
            catch
            {
                return false;
            }   
        }

        public async Task<List<Person>> GetAllPersonsAsync()
        {
            _logger.LogInformation("GetAllPersonsAsync action method of PersonRepository");
            return await _context.Persons.Include(p=>p.Country).ToListAsync();
        }

        public async Task<List<Person>> GetFilteredPersonsAsync(Expression<Func<Person, bool>> predicate)
        {
            return await _context.Persons.Where(predicate).Include(p=>p.Country).ToListAsync();
        }

        public async Task<Person?> GetPersonByPersonIdAsync(int id)
        {
            return await _context.Persons.FindAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Persons.AnyAsync(p => p.PersonId == id);
        }

        public async Task<Person> UpdatePersonAsync(int id, Person person)
        {
            var existingPerson = await GetPersonByPersonIdAsync(id);

            // This validation is delegated to the PersonService as per your design.
            existingPerson.FirstName = person.FirstName;
            existingPerson.LastName = person.LastName;
            existingPerson.Gender = person.Gender;
            existingPerson.CountryId = person.CountryId;
            existingPerson.DateOfBirth = person.DateOfBirth;

            await _context.SaveChangesAsync();
            return existingPerson;
        }

        public async Task<List<Person>> GetSortedAscendingAsync(Expression<Func<Person, object>> keySelector)
        {
            _logger.LogInformation("GetSortedAscendingAsync methods of service PersonService");
            return await _context.Persons.OrderBy(keySelector).ToListAsync();
        }

        public async Task<List<Person>> GetSortedDescendingAsync(Expression<Func<Person, object>> keySelector)
        {
            _logger.LogInformation("GetSortedDescendingAsync methods of service PersonService");
            return await _context.Persons.OrderByDescending(keySelector).ToListAsync();
        }

    }
}
