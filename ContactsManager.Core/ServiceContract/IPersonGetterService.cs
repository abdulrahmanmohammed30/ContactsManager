using ServiceContracts.DTO;
using ServiceContracts.Enums;


namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    ///  Represents business logic for manipulating person entity 
    /// </summary>
    public interface IPersonGetterService
    {
        /// <summary>
        /// Return all persons
        /// </summary>
        /// <returns>Returns a list of objects of PersonResponse type</returns> 
        Task<List<PersonResponse>> GetAllPersonsAsync();

        /// <summary>
        /// Returns the person object based on the given person id 
        /// </summary>
        /// <param name="id">Person id to search</param>
        /// <returns>Returns Matching person object</returns>
        Task<PersonResponse?> GetPersonByPersonIdAsync(int? id);

        /// <summary>
        ///  Returns all person objects that matches with the given 
        ///  search field and search string 
        /// </summary>
        /// <param name="searchBy">Searh field to search</param>
        /// <param name="searchString">Search string to search</param>
        /// <returns>Returns all matching persons based on the given search field and search string</returns>
        Task<List<PersonResponse>> GetFilteredPersonsAsync(string searchBy,
            string? searchString);
        
        Task<bool> Exists(int id);
    }
}
