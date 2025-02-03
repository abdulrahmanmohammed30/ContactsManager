using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContract
{
    public interface IPersonResponseEnricher
    {
        /// <summary>
        /// Adds CountryName to each PersonResponse based on the defined PersonResponse CountryID
        /// </summary>
        /// <param name="persons">It takes a IEnumerable of PersonResponse</param>
        /// <returns>It returns an IEnumerable of PersonResponse Enriched with CountryName</returns>
        IEnumerable<PersonResponse> Enrich(IEnumerable<PersonResponse> persons);
    }
}
    