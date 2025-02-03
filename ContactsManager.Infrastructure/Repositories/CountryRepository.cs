using System.Linq.Expressions;
using Entities;
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryContracts;

namespace ContactsManager.Infrastructure.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ICountryRepository> _logger;
        public CountryRepository(AppDbContext context, ILogger<ICountryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<Country> AddCountryAsync(Country country)
        {
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return country;
        }

        public async Task<List<Country>> GetAllCountriesAsync()
        {
            _logger.LogInformation("GetAllCountriesAsync action method of CountryRepository");
            return await _context.Countries.ToListAsync();
        }

        public async Task<Country?> GetCountryByCountryIdAsync(int countryId)
        {
            return await _context.Countries.FirstOrDefaultAsync(c => c.CountryId == countryId);
        }

        public async Task<bool> AnyAsync(Expression<Func<Country, bool>> predicate)
        {
            // EF Core doesn't support async predicate evaluation directly, so use AsQueryable()
            return await _context.Countries.AsQueryable().AnyAsync(predicate);
        }
    }
}
