using ContactsManager.Core.ServiceContract;
using Microsoft.Extensions.Logging;
using RepositoryContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Core.Services
{
    public class CountryAdderService : ICountryAdderService
    {
        private readonly ICountryRepository _countryRepository;
        private readonly ILogger<CountryAdderService> _logger; 

        public CountryAdderService(ICountryRepository countryRepository, ILogger<CountryAdderService>  logger)
        {
            _countryRepository = countryRepository;
            _logger = logger;
        }

        public async Task<CountryResponse> AddCountryAsync(CountryAddRequest countryAddRequest)
        {
            if (countryAddRequest == null)
                throw new ArgumentNullException(nameof(countryAddRequest));

            if (string.IsNullOrWhiteSpace(countryAddRequest.CountryName))
                throw new ArgumentException($"{nameof(countryAddRequest.CountryName)} cannot be null");

            var country = countryAddRequest.ToCountry();

            if (await _countryRepository.AnyAsync(x => x.CountryName == country.CountryName))
                throw new ArgumentException("Duplicate country name");

            var addedCountry = await _countryRepository.AddCountryAsync(country);

            return addedCountry.ToCountryReponse();
        }
    }
}
