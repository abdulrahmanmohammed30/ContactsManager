using Entities;
using ServiceContracts.DTO;

namespace CrudProject.ViewModels
{
    public class UpdatePersonCountriesViewModel
    {
        public PersonUpdateRequest PersonUpdateRequest { get; set; } 
        public List<CountryResponse> Countries { get; set; }

        public UpdatePersonCountriesViewModel(PersonUpdateRequest personAddRequest, List<CountryResponse> countries) {
            PersonUpdateRequest = personAddRequest;
            Countries = countries;
        }
    }
}
