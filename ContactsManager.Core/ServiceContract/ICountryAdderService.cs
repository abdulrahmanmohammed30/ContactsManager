using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    /// Represents business logic for manipulation country entity
    /// </summary>
    public interface ICountryAdderService
    {
        /// <summary>
        /// <summary>
        /// Adds a country Object into a list of countries 
        /// 
        /// </summary>
        /// <param name="countryAddRequest">
        /// country object to add </param>
        /// <returns>Returns the country object
        /// after adding it(Including newly generated country id} </returns>
        Task<CountryResponse> AddCountryAsync(CountryAddRequest countryAddRequest);
    }
}