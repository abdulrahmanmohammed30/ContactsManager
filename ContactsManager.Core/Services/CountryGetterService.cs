using ContactsManager.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Core.Services
{
    public class CountryGetterService : ICountryGetterService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryGetterService> _logger; 

        public CountryGetterService(ICountryRepository countryRepository, ILogger<CountryGetterService>  logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }
        
        public async Task<List<CountryResponse>> GetAllCountriesAsync()
        {
            _logger.LogInformation("GetAllCountriesAsync action method of CountriesService");

            var countries = await _countryRepository.GetAllCountriesAsync();
            return countries.Select(x => x.ToCountryReponse()).ToList();
        }

        public async Task<CountryResponse?> GetCountryByCountryIdAsync(int? countryId)
        {
            if (countryId == null)
                return null;

            var country = await _countryRepository.GetCountryByCountryIdAsync(countryId.Value);
            return country?.ToCountryReponse();
        }
    }
}
