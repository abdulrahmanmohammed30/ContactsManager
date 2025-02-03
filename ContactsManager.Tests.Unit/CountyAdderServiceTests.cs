using System.Linq.Expressions;
using ContactsManager.Core.ServiceContract;
using ContactsManager.Core.Services;
using Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RepositoryContracts;
using ServiceContracts.DTO;

namespace ContactsManager.Tests.Unit
{
    public class CountyAdderServiceTests
    {
        private readonly ICountryAdderService _sut;
        private readonly ICountryRepository _countryRepository = Substitute.For<ICountryRepository>();
        private readonly ILogger<CountryAdderService> _logger=Substitute.For<ILogger<CountryAdderService>>();

        public CountyAdderServiceTests()
        {
            _sut = new CountryAdderService(_countryRepository, _logger);

        }

        [Fact]
        public async Task AddCountry_ShouldThrowArgumentNullException_WhenCountryAddRequestIsNull()
        {
            // Arrange 
            CountryAddRequest countryAddRequest = null;

            // Act 
            var actionRequest = async () => await _sut.AddCountryAsync(countryAddRequest);

            // Assert 
            await actionRequest.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task AddCountry_ShouldReturnArgumentException_WhenCountryNameIsNull()
        {
            // Arrange 
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = null
            };

            // Act 
            var actionRequest = async () => await _sut.AddCountryAsync(countryAddRequest);

            // Assert 
            await actionRequest.Should().ThrowAsync<ArgumentException>();
        }


        [Fact] // fix error
        public async Task AddCountry_ShouldReturnArgumentException_WhenCountryNameIsDuplicated()
        {
            // Arrange 
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "United Kindom"
            };

            _countryRepository.AnyAsync(Arg.Any<Expression<Func<Country, bool>>>()).Returns(true);


            // Act 
            var actionRequest = async () => await _sut.AddCountryAsync(countryAddRequest);

            // Assert
            // you have to await the execution of the following line
            // otherwise, the test case willl pass even if no exceptions were thrown 
            await actionRequest.Should().ThrowAsync<ArgumentException>();
        }


        [Fact]
        public async Task AddCountry_ShouldReturnCountryResponse_WhenInvokedWithValidCountryAddRequest()
        {
            // Arrange 
            var countryAddRequest = new CountryAddRequest()
            {
                CountryName = "Finland"
            };

            var expectedCountryResponse = new CountryResponse()
            {
                CountryName = countryAddRequest.CountryName
            };

            _countryRepository.AddCountryAsync(
               Arg.Any<Country>()
            ).Returns(new Country()
            {
                CountryId = 1,
                CountryName = "Finland"
            });


            // Act 
            CountryResponse res = await _sut.AddCountryAsync(countryAddRequest);

            // Assert 
            res.CountryID.Should().BeGreaterThan(0);
            res.Should().BeEquivalentTo(expectedCountryResponse,
                options => options.Excluding(x => x.CountryID));
        }
    }
}
