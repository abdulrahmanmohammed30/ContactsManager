using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    ///  Represents business logic for manipulating person entity 
    /// </summary>
    public interface IPersonAdderService
    {
        /// <summary>
        ///  Add new person to the existing list of persons 
        /// </summary>
        /// <param name="personAddRequest">Person to add</param>
        /// <returns>Returns the same person details, along with newly PersonID</returns>
        Task<PersonResponse> AddPersonAsync(PersonAddRequest? personAddRequest);
    }
}
