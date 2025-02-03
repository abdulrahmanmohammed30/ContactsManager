using ServiceContracts.DTO;

namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    ///  Represents business logic for manipulating person entity 
    /// </summary>
    public interface IPersonUpdaterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="personUpdateRequest"></param>
        /// <returns></returns>
         Task<PersonResponse> UpdatePersonAsync(int id, PersonUpdateRequest? personUpdateRequest);
    }
}
