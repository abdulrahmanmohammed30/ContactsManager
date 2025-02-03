using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    /// Represents business logic for manipulation country entity
    /// </summary>
    public interface ICountryGetterService
    {
        /// <summary>
        /// Returns all countries 
        /// </summary>
        /// <returns>All countries from the list as List of CountryResponse</returns>
          Task<List<CountryResponse>> GetAllCountriesAsync();
        
        /// <summary>
        /// Returns a country object based on the given country id 
        /// </summary>
        /// <param name="countryId">CountryID to search</param>
        /// <returns>Matching country as CountryResponse object</returns>
          Task<CountryResponse?> GetCountryByCountryIdAsync(int? countryId);
    }
}
    