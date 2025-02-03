using System.Linq.Expressions;
using ContactsManager.Core.ServiceContract;
using ContactsManager.Core.Services;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RepositoryContracts;
using Serilog;
using ServiceContracts.DTO;
using ServiceContracts.Enums;   
using Services;
using Xunit.Abstractions;


namespace ContactsManager.Tests.Unit
{
    public class PersonUpdaterServiceTests
    {
        private readonly IPersonUpdaterService _sut;

        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger<PersonUpdaterService> _logger= Substitute.For<ILogger<PersonUpdaterService>>();

        private readonly IDiagnosticContext _diagnosticContext = Substitute.For<IDiagnosticContext>();


        public PersonUpdaterServiceTests(ITestOutputHelper testOutputHelper)
        {
            // , _diagnosticContext
            _sut = new PersonUpdaterService(_personRepository, _logger);
            _testOutputHelper = testOutputHelper;
        }
    #region UpdatePerson

        [Fact]
        public async Task UpdatePerson_ShouldThrowException_WhenPersonToUpdateIsNull()
        {
            //  Arrange 
            PersonUpdateRequest? personUpdateRequest = null;

            //  Arrange 

            var actionRequest = async () => await _sut.UpdatePersonAsync(1, personUpdateRequest);

            // Act 
            actionRequest.Should().ThrowAsync<ArgumentNullException>();
        }
        [Fact]
        public void UpdatePerson_ShouldThrowArgumentException_WhenPersonIdIsInvalid()
        {

            //  Arrange 
            PersonUpdateRequest? personUpdateRequest = new PersonUpdateRequest()
            {
                PersonId = 5000
            };

            _personRepository.ExistsAsync(Arg.Any<int>()).Returns(false);

            //  Arrange 

            var actionRequest = () => _sut.UpdatePersonAsync(50000, personUpdateRequest);

            // Act 
            actionRequest.Should().ThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task UpdatePerson_ShoulReturnAnInstanceOFUpdatedPersonData_WhenPersonDataAreValid()
        {
            //  Arrange 
            var personUpdateRequest =
                 new PersonUpdateRequest
                 {
                     FirstName = "updated firstName",
                     LastName = "Smith",
                     DateOfBirth = new DateTime(1990, 6, 15),
                     Gender = 'F',
                     CountryId = 1,
                     ReceiveNewsLetters = true
                 };

            _personRepository.ExistsAsync(Arg.Any<int>()).Returns(true);

            _personRepository.UpdatePersonAsync(Arg.Any<int>(), Arg.Any<Person>())
                .Returns(
                      new Person
                      {
                          PersonId = 1,
                          FirstName = "updated firstName",
                          LastName = "Smith",
                          Email = "alice.smith@example.com",
                          DateOfBirth = new DateTime(1990, 6, 15),
                          Gender = 'F',
                          CountryId = 1,
                          ReceiveNewsLetters = true
                      }
                );

            // Act 
            var res = await _sut.UpdatePersonAsync(1, personUpdateRequest);

            // Assert
            res.Should().BeEquivalentTo(personUpdateRequest,
                options => options.Excluding(p => p.PersonId));
        }
        #endregion

    }
}

