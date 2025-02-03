
using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    public interface IPersonRepository
    {
        Task<Person> AddPersonAsync(Person person);

        Task<List<Person>> GetAllPersonsAsync();

        Task<Person?> GetPersonByPersonIdAsync(int id);

        Task<List<Person>> GetFilteredPersonsAsync(Expression<Func<Person, bool>> predicate);

        Task<Person> UpdatePersonAsync(int id, Person person);

        Task<bool> DeletePersonAsync(int personId);

        Task<bool> ExistsAsync(int id);
        Task<List<Person>> GetSortedDescendingAsync(Expression<Func<Person, object>> keySelector);
        Task<List<Person>> GetSortedAscendingAsync(Expression<Func<Person, object>> keySelector);

    }
}
