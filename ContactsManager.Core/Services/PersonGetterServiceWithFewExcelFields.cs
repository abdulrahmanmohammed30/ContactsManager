using ContactsManager.Core.ServiceContract;
using ServiceContracts.DTO;

namespace ContactsManager.Core.Services;

public class PersonGetterServiceWithFewExcelFields:IPersonGetterService
{
    private readonly PersonGetterService _personGetterService;  

    public PersonGetterServiceWithFewExcelFields(PersonGetterService personGetterService)
    {
        _personGetterService= personGetterService;
    }
    
    public async Task<List<PersonResponse>> GetAllPersonsAsync()
    {
        return await _personGetterService.GetAllPersonsAsync();
    }

    public async Task<PersonResponse?> GetPersonByPersonIdAsync(int? id)
    {
        return await _personGetterService.GetPersonByPersonIdAsync(id);
    }

    public async Task<List<PersonResponse>> GetFilteredPersonsAsync(string searchBy, string? searchString)
    {
        return await _personGetterService.GetFilteredPersonsAsync(searchBy, searchString);
    }

    public async Task<bool> Exists(int id)
    {
        return await _personGetterService.Exists(id);
    }
}