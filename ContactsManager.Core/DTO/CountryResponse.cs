using Entities;

namespace ServiceContracts.DTO
{
    /// <summary>
    /// DTO class that's used as return type for most
    /// of CountryResponse methods 
    /// </summary>
    public class CountryResponse
    {
        public int CountryID { get; set; }
        public string CountryName { get; set; }
    }

    public static class CountryExtensions
    {
        public static CountryResponse ToCountryReponse(this Country country) => new CountryResponse()
        {
            CountryID = country.CountryId,
            CountryName = country.CountryName
        };

    }
}
    