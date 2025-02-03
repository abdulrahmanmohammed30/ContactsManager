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
    public class PersonSorterServiceTests1
    {
        private readonly IPersonSorterService _sut;

        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger<PersonSorterService> _logger= Substitute.For<ILogger<PersonSorterService>>();

        private readonly IDiagnosticContext _diagnosticContext = Substitute.For<IDiagnosticContext>();


        public PersonSorterServiceTests1(ITestOutputHelper testOutputHelper)
        {
            // _diagnosticContext
            _sut = new PersonSorterService(_personRepository, _logger);
            _testOutputHelper = testOutputHelper;
        }
        
        //   when we sort based on PersonName in DESC, it should return persons in descending on PersonName
        [Fact]
        public async Task GetSortedPersons_ShouldReturnDESCSortedPersonsByFirstName_WhenSortCategoryIsByFirstNameAndOptionISDESC()
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

            var expectedResponse = personsAddRequest.OrderByDescending(p => p.FirstName).ToList();

            _personRepository.GetSortedDescendingAsync(Arg.Any<Expression<Func<Person, object>>>()).Returns(expectedResponse.Select(p => p.ToPerson()).ToList());
            // Act 
            var sorted_persons = await _sut.GetSortedPersonsAsync(nameof(Person.FirstName), SortOrderOptions.DESC);

            // Assert
            sorted_persons.Should().BeEquivalentTo(expectedResponse);
            int sz = sorted_persons.Count();

            if (sorted_persons.Count() != expectedResponse.Count())
            {
                throw new Exception("Lists length are not equal");
            }

            for (int i = 0; i < sz; i++)
            {
                sorted_persons[i].Should().BeEquivalentTo(expectedResponse[i]);

            }
        }
    }
}

