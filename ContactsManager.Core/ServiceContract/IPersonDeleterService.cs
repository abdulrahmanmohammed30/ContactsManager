using ServiceContracts.DTO;
using ServiceContracts.Enums;


namespace ContactsManager.Core.ServiceContract
{
    /// <summary>
    ///  Represents business logic for manipulating person entity 
    /// </summary>
    public interface IPersonDeleterService
    {
         Task<bool> DeletePersonAsync(int? personId);
    }
}
