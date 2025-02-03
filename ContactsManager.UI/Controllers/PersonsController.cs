using CrudProject.Filters.ResourceFilters;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using System.Text.Json;
using ContactsManager.Core.ServiceContract;


namespace CrudProject.Controllers
{
    [Route("[controller]")]
    //[TypeFilter(typeof(HandleExceptionFilter))]
    //[TypeFilter(typeof(PersonAlwaysRunResultFilter))]
    public class PersonsController : Controller
    {
        private readonly IPersonGetterService _personGetterService;
        private readonly IPersonSorterService _personSorterService;
        private readonly IPersonAdderService _personAdderService;
        private readonly IPersonUpdaterService _personUpdaterService;
        private readonly IPersonDeleterService _personDeleterService;
        private readonly ICountryGetterService _countryGetterService;

        public PersonsController(IPersonGetterService personGetterService, IPersonSorterService personSorterService,IPersonAdderService personAdderService, 
            IPersonUpdaterService personUpdaterService, IPersonDeleterService personDeleterService,  ICountryGetterService countryGetterService)
        {
            _personGetterService = personGetterService;
            _personSorterService = personSorterService;
            _personAdderService = personAdderService;
            _personUpdaterService = personUpdaterService;
            _personDeleterService = personDeleterService;
            _countryGetterService = countryGetterService;
        }

        [HttpGet("/")]
       // [ExecutionTimeActionFilterFactory(-1)]
        //[TypeFilter(typeof(CookieAuthorizationFilter))]
        //[TypeFilter(typeof(FeatureDisableResourceFilter), Arguments = new object[] { false })]
        //[TypeFilter(typeof(FeatureDisableResourceFilter))]
        //[TypeFilter(typeof(TokenResultFilter))]
        // [TypeFilter(typeof(HandleExceptionFilter))]
        //[TypeFilter(typeof(FeatureDisableResourceFilter))]
        //[TypeFilter(typeof(DataActionFilters))]
        //[TypeFilter(typeof(TokenResultFilter))]
        //[TypeFilter(typeof(PersonAlwaysRunResultFilter))]
        //[SkipFilter]
        public async Task<IActionResult> Index()
        {
            var persons = await _personGetterService.GetAllPersonsAsync();
            return View(persons);
        }


        [HttpGet("data")]
        public async Task<IActionResult> GetPersonsData()
        {
            var persons = await _personGetterService.GetAllPersonsAsync();
            return Ok(persons);
        }

        [HttpGet("sort")]
        public async Task<IActionResult> GetSortedData(string sortColumn, string sortOrder)
        {
            try
            {
                List<PersonResponse> persons;

                if (string.Compare(sortOrder, "asc", true) == 0)
                    persons = await _personSorterService.GetSortedPersonsAsync(sortColumn, SortOrderOptions.ASC);
                else
                    persons = await _personSorterService.GetSortedPersonsAsync(sortColumn, SortOrderOptions.DESC);

                return Ok(persons);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
    }

        [HttpGet("filter")]
        public async Task<IActionResult> GetFilteredData(string property, string searchTerm)
        {
            try
            {
                var persons = await _personGetterService.GetFilteredPersonsAsync(property, searchTerm);
                return Ok(persons);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }

        [HttpGet("[action]")]
        [TypeFilter(typeof(FeatureDisableResourceFilter))]
        public async Task<IActionResult> Create()
        {   
            var countries = await _countryGetterService.GetAllCountriesAsync();
            TempData["countries"] = JsonSerializer.Serialize(countries);

            ViewBag.countries = countries;
            return View(new PersonAddRequest());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePerson(PersonAddRequest personAddRequest)
        {
            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries;
                if (TempData.TryGetValue("countries", out object? countriesObj) && countriesObj != null &&
                    JsonSerializer.Deserialize<List<CountryResponse>>(countriesObj.ToString() ?? "") is List<CountryResponse> deserializedCountries)
                {
                    countries = deserializedCountries;
                    TempData.Keep();
                }
                else
                {
                    countries = await _countryGetterService.GetAllCountriesAsync();
                    TempData["countries"] = JsonSerializer.Serialize(countries);
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
                ViewBag.errors = errors;
                ViewBag.countries = countries;
                return View("create", personAddRequest);
            }

            var person = await _personAdderService.AddPersonAsync(personAddRequest);

            if (person == null)
            {
                return StatusCode(500, "An error has occurred when adding a person");
            }

            return RedirectToAction("Index");
        }

        [HttpGet("update/{id}")]
        //[TypeFilter(typeof(TokenResultFilter))]

        public async Task<IActionResult> Update(int id)
        {
            if (!await _personGetterService.Exists(id)) return NotFound($"User with id: {id} was not found.");

            var person = await _personGetterService.GetPersonByPersonIdAsync(id);
            /*
            if (person == null)
            {
                // remove exception;
                throw new InvalidPersonIdException("Exception for testing");
                //return NotFound($"User with id: {id} was not found.");
            }
            */

            var countries = await _countryGetterService.GetAllCountriesAsync();
            TempData["countries"] = JsonSerializer.Serialize(countries);
            ViewBag.countries = countries;

            return View(person.ToUpdateRequest());
        }

        [HttpPost("update/{id}")]
        //[TypeFilter(typeof(TokenAuthorizationFilter))]
        public async Task<IActionResult> Update(int id, PersonUpdateRequest personUpdateRequest)
        {
            var person = await _personGetterService.GetPersonByPersonIdAsync(personUpdateRequest.PersonId);
            if (person == null)
            {
                return NotFound($"User with id: {personUpdateRequest.PersonId} was not found.");
            }

            if (!ModelState.IsValid)
            {
                List<CountryResponse> countries;
                if (TempData.TryGetValue("countries", out object? countriesObj) && countriesObj != null &&
                    JsonSerializer.Deserialize<List<CountryResponse>>(countriesObj.ToString() ?? "") is List<CountryResponse> deserializedCountries)
                {
                    countries = deserializedCountries;
                    TempData.Keep();
                }
                else
                {
                    countries = await _countryGetterService.GetAllCountriesAsync();
                    TempData["countries"] = JsonSerializer.Serialize(countries);
                }

                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(v => v.ErrorMessage);
                ViewBag.errors = errors;
                ViewBag.countries = countries;
                return View(personUpdateRequest);
            }

            var personResponse = await _personUpdaterService.UpdatePersonAsync(id, personUpdateRequest);

            if (personResponse == null)
                return StatusCode(500, "An error has occurred when updating person data");

            return RedirectToAction("Index");
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _personGetterService.Exists(id)) return NotFound($"User with id: {id} was not found.");
            
            var deleted = await _personDeleterService.DeletePersonAsync(id);
            
            if (!deleted)
            {
                var errorResponse = new { message = $"An error has occurred during deleting user with id: {id}" };
                return StatusCode(500, errorResponse);
            }

            return RedirectToAction("Index");
        }

        [HttpGet("PersonsPDF")]
        public async Task<IActionResult> PersonsPDF()
        {
            // return the view as PDF 

            var persons = await _personGetterService.GetAllPersonsAsync();

            return new ViewAsPdf("PersonsPDF", persons, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() { Top = 20, Right = 20, Left = 20, Bottom = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }

}
