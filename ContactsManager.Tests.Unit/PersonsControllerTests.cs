using System.Text.Json;
using AutoFixture;
using ContactsManager.Core.ServiceContract;
using CrudProject.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using NSubstitute.ReturnsExtensions;
using ServiceContracts.DTO;
using ServiceContracts.Enums;
using Services;

namespace ContactsManager.Tests.Unit
{
    public class PersonsControllerTests
    {
        private readonly PersonsController _sut;
        private readonly IPersonGetterService _personGetterService = Substitute.For<IPersonGetterService>();
        private readonly IPersonSorterService _personSorterService = Substitute.For<IPersonSorterService>();
        private readonly IPersonAdderService _personAdderService = Substitute.For<IPersonAdderService>();
        private readonly IPersonUpdaterService _personUpdaterService = Substitute.For<IPersonUpdaterService>();
        private readonly IPersonDeleterService _personDeleterService = Substitute.For<IPersonDeleterService>();

        private readonly ICountryGetterService _countriesService = Substitute.For<ICountryGetterService>();
        private readonly ILogger<PersonsController> _logger = Substitute.For<ILogger<PersonsController>>();

        private readonly IFixture _fixture;
        public PersonsControllerTests()
        {
            //_logger
            _sut = new PersonsController(_personGetterService,  _personSorterService, _personAdderService, 
                 _personUpdaterService, _personDeleterService,   _countriesService);
            _fixture = new Fixture();
        }
        
        


        // Helpers 
        private Predicate<string> IsValidPropertyName = property =>
        typeof(PersonResponse).Properties().Any(p => p.Name.Equals(property, StringComparison.OrdinalIgnoreCase));

        [Fact]
        public async Task Index_ShouldReturnViewResult_WhenInvoked()
        {
            // Arrange 
            var personsResponse = _fixture.CreateMany<PersonResponse>().ToList();
            _personGetterService.GetAllPersonsAsync().Returns(personsResponse);

            // Act 
            var res = (ViewResult)await _sut.Index();

            // Assert 

            // I knew that I did not provide the name in the index view 
            // since the Aps.net will search for a view with the name of the action method 
            // So I want to make sure that I am calling the index view and not different view 
            // to do so I will check if the view name is null or empty and if it's then 
            // by default the action method will call the index 

            // This validates that the action returns a ViewResult
            res.ViewName.Should().BeNullOrEmpty();
            res.Model.Should().BeAssignableTo<IEnumerable<PersonResponse>>();
            res.Model.Should().BeEquivalentTo(personsResponse);
        }

        [Fact]
        public async Task GetPersonsData_ShouldReturnPersonResponseList_WhenUsersExist()
        {
            // Arrange 
            var personsResponse = _fixture.CreateMany<PersonResponse>().ToList();
            _personGetterService.GetAllPersonsAsync().Returns(personsResponse);

            // Act 
            var res = (OkObjectResult)await _sut.GetPersonsData();

            // Assert   
            res.StatusCode.Should().Be(200);
            res.Value.Should().BeEquivalentTo(personsResponse);
        }

        [Theory]
        [InlineData("FirstName", SortOrderOptions.ASC)]
        [InlineData("FirstName", SortOrderOptions.DESC)]
        public async Task GetSortedData_ShouldReturnPersonResponsesSortedList_WhenSortColumnIsValid(string sortColumn, SortOrderOptions sortOrder)
        {
            // Arrange 
            var personsResponse = _fixture.CreateMany<PersonResponse>().ToList();
            _personSorterService.GetSortedPersonsAsync(Arg.Is<string>((x => IsValidPropertyName(x))),
                Arg.Any<SortOrderOptions>()).Returns(personsResponse);

            // Act 
            var res = (OkObjectResult)await _sut.GetSortedData(sortColumn, "asc");

            // Assert 
            res.StatusCode.Should().Be(200);
            res.Value.Should().BeEquivalentTo(personsResponse);
        }

        [Theory]
        [InlineData("invalidColumnName", SortOrderOptions.ASC)]
        public async Task GetSortedData_ShouldReturnBadRequest_WhenSortColumnIsInvalid(string sortColumn, SortOrderOptions sortOrder)
        {
            // Arrange 
            var expectedError = new { error = $"The sort column '{sortColumn}' does not exist on the Person object." };

            _personSorterService.GetSortedPersonsAsync(Arg.Any<string>(),
                Arg.Any<SortOrderOptions>()).Throws(new ArgumentException(expectedError.error));

            // Act 
            // sortOrder
            var res = (BadRequestObjectResult)await _sut.GetSortedData(sortColumn, "asc");

            // Assert 
            res.StatusCode.Should().Be(400);
            res.Value.Should().BeEquivalentTo(expectedError, options => options.ComparingByMembers<object>());
        }


        [Theory]
        [InlineData("firstName", "t")]
        [InlineData("firstName", "")]
        public async Task GetFilteredData_ShouldReturnFilteredPersonResponseList_WhenPropertyIsValid(string property, string searchTerm)
        {
            // Arrange 
            var personsResponse = _fixture.CreateMany<PersonResponse>().ToList();
            _personGetterService.GetFilteredPersonsAsync(Arg.Any<string>(),
                Arg.Any<string>()).Returns(personsResponse);

            // Act 
            var res = (OkObjectResult)await _sut.GetFilteredData(property, searchTerm);

            // Assert 
            res.StatusCode.Should().Be(200);
            res.Value.Should().BeEquivalentTo(personsResponse);
        }

        [Theory]
        [InlineData("invalidColumnName", "")]
        public async Task GetFilteredData_ShouldReturnNull_WhenPropertyIsInvalid
            (string property, string searchTerm)
        {
            // Arrange 
            var expectedError = new { error = $"{property} is not a valid property name." };

            _personGetterService.GetFilteredPersonsAsync(Arg.Any<string>(),
                Arg.Any<string>()).Throws(new ArgumentException(expectedError.error));

            // Act 
            var res = (BadRequestObjectResult)await _sut.GetFilteredData(property, searchTerm);

            // Assert 
            res.StatusCode.Should().Be(400);
            res.Value.Should().BeEquivalentTo(expectedError);
        }

        [Fact]
        public async Task Create_ShouldReturnViewResult_WhenInvoked()
        {

            // Arrange 
            var countries = _fixture.CreateMany<CountryResponse>().ToList();
            _countriesService.GetAllCountriesAsync().Returns(countries);

            SetTempData(countries);

            // Act 
            var res = (ViewResult)await _sut.Create();

            res.Should().BeAssignableTo<ViewResult>();
            res.ViewName.Should().BeNullOrEmpty();
            res.Model.Should().BeEquivalentTo(new PersonAddRequest());
            res.ViewData["countries"].Should().BeEquivalentTo(countries);
        }

        [Fact]
        public async Task CreatePerson_ShouldReturnRedirectToAction_WhenInvokedWithValidPersonAddRequest()
        {
            // Arrange 
            var personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "person30@gmail.com")
                .With(p => p.Gender, 'm')
                .Create();
            var person = _fixture.Create<PersonResponse>();
            _personAdderService.AddPersonAsync(Arg.Any<PersonAddRequest>()).Returns(person);

            // Act 
            var res = (RedirectToActionResult)await _sut.CreatePerson(new PersonAddRequest());

            // Assert
            res.ActionName.Should().Be("Index");
        }

        private void SetTempData(List<CountryResponse> countries)
        {
            var tempdata = new TempDataDictionary(new DefaultHttpContext(), Substitute.For<ITempDataProvider>())
            {
                ["countries"] = JsonSerializer.Serialize(countries)
            };

            _sut.TempData = tempdata;
        }

        [Fact]
        public async Task CreatePerson_ShouldReturnViewResult_WhenPersonAddRequestIsInvalid()
        {
            // Arrange 
            // the following is an invalid personAddRequest, since values for email and gender 
            // will not be valid 
            var personAddRequest = new PersonAddRequest
            {
                Email = "invalid-email", // Invalid email format
            };

            var countries = _fixture.CreateMany<CountryResponse>().ToList();
            _countriesService.GetAllCountriesAsync().Returns(countries);

            SetTempData(countries);

            _sut.ModelState.AddModelError("Email", "Invalid email format.");
            _sut.ModelState.AddModelError("Gender", "Gender must be 0 or 1.");

            // Act 
            var res = (ViewResult)await _sut.CreatePerson(personAddRequest);

            // Assert
            res.ViewName.Should().Be("create");
            res.Model.Should().BeEquivalentTo(personAddRequest);
            res.ViewData["countries"].Should().BeEquivalentTo(countries);
        }

        [Fact]
        public async Task CreatePerson_ShouldReturnInternalServerError_WhenCannotCreatePersonOnTheServer()
        {
            // Arrange 
            // the following is an invalid personAddRequest, since values for email and gender 
            // will not be valid 
            var personAddRequest = _fixture.Build<PersonAddRequest>()
                .With(p => p.Email, "person30@gmail.com")
                .With(p => p.Gender, 'm')
                .Create();
            var person = _fixture.Create<PersonResponse>();
            _personAdderService.AddPersonAsync(Arg.Any<PersonAddRequest>()).ReturnsNull();

            SetTempData(new List<CountryResponse>());

            // Act 
            var res = (ObjectResult)await _sut.CreatePerson(personAddRequest);

            // Assert
            res.StatusCode.Should().Be(500);
        }

        [Theory]
        [InlineData(3)]
        public async Task Update_ShouldReturnViewResult_WhenPersonExists(int id)
        {
            // Arrange 
            var person = _fixture.Build<PersonResponse>()
                .With(p => p.Email, "person30@gmail.com")
                .With(p => p.Gender, 'm')
                .Create();
            _personGetterService.Exists(Arg.Any<int>()).Returns(true);
            _personGetterService.GetPersonByPersonIdAsync(Arg.Is<int>(x => x == id)).Returns(person);

            var countries = _fixture.CreateMany<CountryResponse>().ToList();
            _countriesService.GetAllCountriesAsync().Returns(countries);

            SetTempData(countries);

            // Act 
            var res = (ViewResult)await _sut.Update(id);

            // Assert 
            res.Model.Should().BeEquivalentTo(person.ToUpdateRequest());
            res.ViewData["countries"].Should().BeEquivalentTo(countries);

        }

        [Fact]
        public async Task Update_ShouldReturnNotFound_WhenPersonDoesNotExists()
        {
            //Arrange 
            _personGetterService.GetPersonByPersonIdAsync(Arg.Any<int>()).ReturnsNull();

            // Act 
            var res = (NotFoundObjectResult)await _sut.Update(6);

            // Assert
            res.Value.Should().Be("User with id: 6 was not found.");
        }

        [Fact]
        public async Task UpdatePostMethod_ShouldReturnNotFound_WhenPersonDoesNotExists()
        {
            //Arrange 
            int id = 6;
            _personGetterService.Exists(Arg.Any<int>()).Returns(false);

            // Act 
            var res = (NotFoundObjectResult)await _sut.Update(id, new PersonUpdateRequest() {PersonId = id});

            // Assert
            res.Value.Should().Be($"User with id: {id} was not found.");
        }

        [Fact]
        public async Task Update_ShouldReturnRedirectToAction_WhenPersonUpdateRequestIsValid()
        {
            // Arrange 
            var person = _fixture.Create<PersonResponse>();
            _personGetterService.Exists(Arg.Any<int>()).Returns(true);
            _personGetterService.GetPersonByPersonIdAsync(Arg.Any<int>()).Returns(person);

            _personUpdaterService.UpdatePersonAsync(Arg.Any<int>(), Arg.Any<PersonUpdateRequest>()).Returns(person);

            // Act 
            var res = (RedirectToActionResult)await _sut.Update(3, person.ToUpdateRequest());

            // Assert
            res.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Update_ShouldReturnInternalServerError_WhenCannotUpdatePerson()
        {
            // Arrange 
            var person = _fixture.Create<PersonResponse>();
            _personGetterService.Exists(Arg.Any<int>()).Returns(true);
            _personGetterService.GetPersonByPersonIdAsync(Arg.Any<int>()).Returns(person);

            _personUpdaterService.UpdatePersonAsync(Arg.Any<int>(), Arg.Any<PersonUpdateRequest>())
                .ReturnsNull();

            // Act 
            var res = (ObjectResult)await _sut.Update(3, person.ToUpdateRequest());

            // Assert
            res.StatusCode.Should().Be(500);
            res.Value.Should().Be("An error has occurred when updating person data");
        }

        [Fact]
            public async Task Deleted_ShouldReturnRedirectToAction_WhenPersonDeleted()
        {
            // Arrange 
            _personDeleterService.DeletePersonAsync(Arg.Any<int>()).Returns(true);
            _personGetterService.Exists(Arg.Any<int>()).Returns(true);

            int id = 3;

            // Act 
            var res = (RedirectToActionResult)await _sut.Delete(id);

            // Assert 
            res.ActionName.Should().Be("Index");
        }

        [Fact]
        public async Task Deleted_ShouldReturnNotFound_WhenCannotDeletePerson()
        {
            // Arrange 
            _personGetterService.Exists(Arg.Any<int>()).Returns(false);

            int id = 3;

            var errorResponse = $"User with id: {id} was not found.";

            // Act 
            var res = (NotFoundObjectResult)await _sut.Delete(id);

            // Assert 
            res.StatusCode.Should().Be(404);
            res.Value.Should().BeEquivalentTo(errorResponse);
        }

        [Fact]
        public async Task Deleted_ShouldReturnInternalServerError_WhenCannotDeletePerson()
        {
            // Arrange 
            _personDeleterService.DeletePersonAsync(Arg.Any<int>()).Returns(false);
            _personGetterService.Exists(Arg.Any<int>()).Returns(true);

            int id = 3;

            var errorResponse = new { message = $"An error has occurred during deleting user with id: {id}" };

            // Act 
            var res = (ObjectResult)await _sut.Delete(id);

            // Assert 
            res.StatusCode.Should().Be(500);
            res.Value.Should().BeEquivalentTo(errorResponse);
        }
    }
}


