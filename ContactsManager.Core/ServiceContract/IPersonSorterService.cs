using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    ///  Represents business logic for manipulating person entity 
    /// </summary>
    public interface IPersonSorterService
    {
        /// <summary>
        /// Returns sorted list of persons 
        /// </summary>
        /// <param name="allPersons">Represents list of persons to sort</param>
        /// <param name="sortBy">Name of the property (key), based on which the persons should be sorted</param>
        /// <param name="options">ASC or DESC</param>
        /// <returns>Returns sorted persons as PersonsResponse list</returns>
        Task<List<PersonResponse>> GetSortedPersonsAsync(string sortBy, SortOrderOptions options);
    }
}
