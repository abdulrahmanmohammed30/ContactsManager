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
    public class CountyGetterServiceTests
    {
        private readonly ICountryGetterService _sut;
        private readonly ICountryRepository _countryRepository = Substitute.For<ICountryRepository>();
        private readonly ILogger<CountryGetterService> _logger=Substitute.For<ILogger<CountryGetterService>>();

        public CountyGetterServiceTests()
        {
            _sut = new CountryGetterService(_countryRepository, _logger);

        }
        
        #region GetAllCountries

        /// <summary>
        /// The list of countries should be empty by default 
        /// (before adding any countries)
        /// </summary>
        [Fact]
        public async Task GetAllCountries_ShouldBeEmpty_WhenNoCountriesWereAddedYet()
        {
            // Arrange 
            _countryRepository.GetAllCountriesAsync().Returns(Enumerable.Empty<Country>().ToList());

            // Act 
            var res = await _sut.GetAllCountriesAsync();

            // Assert 
            res.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllCountries_ShouldReturnAllCountries_WhenAddingFewCountries()
        {
            // Arrange 
            List<CountryAddRequest> countries = new List<CountryAddRequest>
                {
                    new CountryAddRequest { CountryName = "United States" },
                    new CountryAddRequest { CountryName = "Canada" },
                    new CountryAddRequest { CountryName = "United Kingdom" },
                    new CountryAddRequest { CountryName = "Australia" },
                    new CountryAddRequest { CountryName = "Germany" },
                    new CountryAddRequest { CountryName = "France" },
                    new CountryAddRequest { CountryName = "Japan" },
                    new CountryAddRequest { CountryName = "India" },
                    new CountryAddRequest { CountryName = "Brazil" },
                    new CountryAddRequest { CountryName = "South Africa" }
                };

            var countriesResponse = new List<CountryResponse>(countries.Select(c => new CountryResponse()
            {
                CountryName = c.CountryName
            }));

            _countryRepository.GetAllCountriesAsync()
                .Returns(countries.Select(c => c.ToCountry()).ToList());

            // Act 
            var res = await _sut.GetAllCountriesAsync();

            // Assert 
            res.Should().BeEquivalentTo(countriesResponse, options => options.Excluding(c => c.CountryID));
        }

        #endregion

        #region GetCountryByCountryId

        [Fact]
        public async Task GetCountryByCountryId_ShouldReturnNull_WhenCountryIdParameterIsNull()
        {
            // Arrange 
            int? countryId = null;

            // Act 
            var res = await _sut.GetCountryByCountryIdAsync(countryId);

            // Assert 
            res.Should().BeNull();
        }


        [Fact]
        public async Task GetCountryByCountryId_ShouldReturnCountryResponse_WhenCountryIsValidAndCountryExists()
        {
            // Arrange 
            var country = new Country()
            {
                CountryName = "Finland"
            };

            _countryRepository.GetCountryByCountryIdAsync(Arg.Any<int>()).Returns(country);

            // Act 
            var res = await _sut.GetCountryByCountryIdAsync(1);

            // Assert 
            res.CountryName.Should().Be("Finland");
        }
        #endregion

    }

    internal class CountyGetterService
    {
    }
}
