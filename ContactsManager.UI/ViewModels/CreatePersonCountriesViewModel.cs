using Entities;
using ServiceContracts.DTO;

namespace CrudProject.ViewModels
{
    public class CreatePersonCountriesViewModel
    {
        public PersonAddRequest PersonAddRequest { get; set; } 
        public List<CountryResponse> Countries { get; set; }

        public CreatePersonCountriesViewModel(PersonAddRequest personAddRequest, List<CountryResponse> countries) {
            PersonAddRequest = personAddRequest;
            Countries = countries;
        }
    }
}
