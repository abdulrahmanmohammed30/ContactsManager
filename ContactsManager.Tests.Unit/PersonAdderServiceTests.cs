using Entities;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;
using ContactsManager.Core.ServiceContract;
using ContactsManager.Core.Services;
using FluentAssertions;

namespace ContactsManager.Tests.Unit
{
    public class PersonAdderServiceTests
    {
        private readonly IPersonAdderService _sut;

        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger<PersonAdderService> _logger= Substitute.For<ILogger<PersonAdderService>>();

        private readonly IDiagnosticContext _diagnosticContext = Substitute.For<IDiagnosticContext>();


        public PersonAdderServiceTests(ITestOutputHelper testOutputHelper)
        {
            // , _diagnosticContext
            _sut = new PersonAdderService(_personRepository, _logger);
            _testOutputHelper = testOutputHelper;
        }
        
        [Fact]
        public async Task AddPerson_ShouldThrowsArgumentNullException_WhenPersonIsNull()
        {
            // AddPerson GetAllPersons
            //  Arrange 
            PersonAddRequest? personAddRequest = null;

            // Act 
            var requestAction = async () => await _sut.AddPersonAsync(personAddRequest);

            // Assert
            await requestAction.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddPerson_ShouldThrowsArugmentException_WhenPersonFirstNameIsNull()
        {
            // AddPerson GetAllPersons
            //  Arrange 
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                FirstName = null
            };

            // Act 
            var requestAction = async () => await _sut.AddPersonAsync(personAddRequest);

            // Assert
            await requestAction.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task AddPerson_ShouldAddPerson_WhenPersonDataAreValid()
        {
            // AddPerson GetAllPersons
            //  Arrange 
            PersonAddRequest? personAddRequest =
            new PersonAddRequest
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                DateOfBirth = new DateTime(1990, 6, 15),
                Gender = 'F',
                CountryId = 1,
                ReceiveNewsLetters = true
            };

            var expectedPersonResponse = new PersonResponse()
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                DateOfBirth = new DateTime(1990, 6, 15),
                Gender = 'F',
                CountryId = 1,
                ReceiveNewsLetters = true
            };

            _personRepository.AddPersonAsync(Arg.Any<Person>()).Returns(new Person()
            {
                PersonId = 1,
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                DateOfBirth = new DateTime(1990, 6, 15),
                Gender = 'F',
                CountryId = 1,
                ReceiveNewsLetters = true
            });

            // Act 
            var actualPersonResponse = await _sut.AddPersonAsync(personAddRequest);

            // Assert
            actualPersonResponse.PersonId.Should().BeGreaterThan(0);
            actualPersonResponse.Should().BeEquivalentTo(expectedPersonResponse, options => options.Excluding(x => x.PersonId));
        }
    }
}

