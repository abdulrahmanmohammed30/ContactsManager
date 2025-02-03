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
using Services;
using Xunit.Abstractions;

namespace ContactsManager.Tests.Unit
{
    public class PersonGetterServiceTests
    {
        private readonly IPersonGetterService _sut;

        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger<PersonGetterService> _logger= Substitute.For<ILogger<PersonGetterService>>();

        private readonly IDiagnosticContext _diagnosticContext = Substitute.For<IDiagnosticContext>();


        public PersonGetterServiceTests(ITestOutputHelper testOutputHelper)
        {
            // , _diagnosticContext
            _sut = new PersonGetterService(_personRepository, _logger);
            _testOutputHelper = testOutputHelper;
        }

        //    #region GetPersonByPersonId 
        [Fact]
        public async Task GetPersonByPersonId_ShouldReturnsNull_WhenPersonIdIsNull()
        {
            // Arrange 
            int? personId = null;

            // Act 
            var personResponse = await _sut.GetPersonByPersonIdAsync(personId);

            // Validate 
            personResponse.Should().BeNull();
        }

        [Fact]
        public async Task GetPersonByPersonId_ShouldReturnsPersonData_WhenPersonIdIsValid()
        {

            PersonResponse expectedPersonResponse =
                new PersonResponse
                {
                    PersonId = 1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    DateOfBirth = new DateTime(1990, 6, 15),
                    Gender = 'F',
                    CountryId = 1,
                    ReceiveNewsLetters = true
                };

            _personRepository.GetPersonByPersonIdAsync(1).Returns(new Person()
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
            var personResponse = await _sut.GetPersonByPersonIdAsync(1);

            // Validate 
            personResponse.Should().BeEquivalentTo(expectedPersonResponse);
        }

        //    #endregion

        //    #region GetAllPersons 

        [Fact]
        public async Task GetAllPersons_ShouldReturnPersonsList_WherePersonsExists()
        {

            //  Arrange 
            var personsData = new List<Person>()
                {
                     new Person
                {
                         PersonId=1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    DateOfBirth = new DateTime(1990, 6, 15),
                    Gender = 'F',
                    CountryId = 1,
                    ReceiveNewsLetters = true
                },

                    new Person
                    {
                        PersonId=2,
                        FirstName = "Bob",
                        LastName = "Johnson",
                        Email = "bob.johnson@example.com",
                        DateOfBirth = new DateTime(1985, 11, 3),
                        Gender = 'M',
                        CountryId = 2,
                        ReceiveNewsLetters = false
                    }

            };
            _personRepository.GetAllPersonsAsync().Returns(personsData);
            _testOutputHelper.WriteLine("Expected: \n");
            _testOutputHelper.WriteLine("*****************************************");
            personsData.ForEach(p =>
            {
                _testOutputHelper.WriteLine($"Name:{p.FirstName} {p.LastName}\nCountryId:{p.CountryId}\nReceiveNewsLetters:{p.ReceiveNewsLetters}\n");
            });

            _testOutputHelper.WriteLine("*****************************************");
            // Act 
            var personsResponse = await _sut.GetAllPersonsAsync();

            _testOutputHelper.WriteLine("\nActual: \n");
            _testOutputHelper.WriteLine("*****************************************");
            personsResponse.ForEach(p =>
            {
                _testOutputHelper.WriteLine($"Name:{p.FirstName} {p.LastName}\nCountryId:{p.CountryId}\nReceiveNewsLetters:{p.ReceiveNewsLetters}\n");
            });

            _testOutputHelper.WriteLine("*****************************************");

            // Assert
            personsResponse.Should().BeEquivalentTo(personsData.Select(p => p.ToPersonResponse()));
        }

        //    #endregion

        //    #region GetFilteredPersons 

        [Fact]
        public async Task GetFilteredPersons_ShouldReturnAllPersons_WhenSearchTextIsEmpty()
        {

            //  Arrange 
            var personsData = new List<Person>()
                {
                     new Person
                {
                    PersonId=1,
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    DateOfBirth = new DateTime(1990, 6, 15),
                    Gender = 'F',
                    CountryId = 1,
                    ReceiveNewsLetters = true
                },

                    new Person
                    {
                        PersonId=2,
                        FirstName = "Bob",
                        LastName = "Johnson",
                        Email = "bob.johnson@example.com",
                        DateOfBirth = new DateTime(1985, 11, 3),
                        Gender = 'M',
                        CountryId = 2,
                        ReceiveNewsLetters = false
                    }

            };

            _personRepository.GetFilteredPersonsAsync(Arg.Any<Expression<Func<Person, bool>>>()).Returns(personsData);

            // Assert 

            var personsResponse = await _sut.GetFilteredPersonsAsync(nameof(Person.FirstName), "");

            personsResponse.ForEach(p =>
            {
                _testOutputHelper.WriteLine($"Name:{p.FirstName} " +
                    $"{p.LastName}\nCountryId:{p.CountryId}\nReceiveNewsLetters:{p.ReceiveNewsLetters}\n");
            });

            // Assert
            personsResponse.Should().BeEquivalentTo(personsData.Select(p => p.ToPersonResponse()));
        }

        [Fact]
        public async Task GetFilteredPersons_ShouldReturnFilteredPerson_WhenSearchTextIsNotEmpty()
        {

            //  Arrange 
            var personsAddRequest = new List<PersonAddRequest>()
                {
                     new PersonAddRequest
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    Email = "alice.smith@example.com",
                    DateOfBirth = new DateTime(1990, 6, 15),
                    Gender = 'F',
                    CountryId = 1,
                    ReceiveNewsLetters = true
                },

                    new PersonAddRequest
                    {
                        FirstName = "Bob",
                        LastName = "Johnson",
                        Email = "bob.johnson@example.com",
                        DateOfBirth = new DateTime(1985, 11, 3),
                        Gender = 'M',
                        CountryId = 2,
                        ReceiveNewsLetters = false
                    }

            };
            _personRepository.GetFilteredPersonsAsync(Arg.Any<Expression<Func<Person, bool>>>()).Returns(
                 personsAddRequest.Where(p => p.FirstName.ToLower().Contains("al")).Select(p => p.ToPerson()).ToList()
                );

            // Act 
            var personsResponse = await _sut.GetFilteredPersonsAsync(nameof(Person.FirstName), "al");

            if (personsResponse.Count != 1)
                throw new Exception("Expected to return one result but there were no results");

            // Assert
            foreach (var personResponse in personsResponse)
            {

                if (!personResponse.FirstName.Contains("al", StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Result doesn't contain searchterm");
            }
            personsResponse.Should().HaveCount(1, "because only one person matches the search criteria");
            personsResponse.Should().OnlyContain(p =>
                p.FirstName.Contains("al", StringComparison.OrdinalIgnoreCase),
                "because all returned persons should match the search criteria");
        }
    }
}

