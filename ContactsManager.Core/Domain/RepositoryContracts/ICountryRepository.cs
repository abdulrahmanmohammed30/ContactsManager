using Entities;
using System.Linq.Expressions;

namespace RepositoryContracts
{
    public interface ICountryRepository
    {
        Task<Country> AddCountryAsync(Country country);

        Task<List<Country>> GetAllCountriesAsync();

        Task<Country?> GetCountryByCountryIdAsync(int countryId);

        Task<bool> AnyAsync(Expression<Func<Country, bool>> predicate);
    }
}
