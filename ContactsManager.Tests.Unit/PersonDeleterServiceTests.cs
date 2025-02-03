using ContactsManager.Core.ServiceContract;
using ContactsManager.Core.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using RepositoryContracts;
using Serilog;
using Services;
using Xunit.Abstractions;

namespace ContactsManager.Tests.Unit
{
    public class PersonDeleterServiceTests
    {
        private readonly IPersonDeleterService _sut;

        private readonly IPersonRepository _personRepository = Substitute.For<IPersonRepository>();

        private readonly ITestOutputHelper _testOutputHelper;

        private readonly ILogger<PersonDeleterService> _logger= Substitute.For<ILogger<PersonDeleterService>>();

        private readonly IDiagnosticContext _diagnosticContext = Substitute.For<IDiagnosticContext>();


        public PersonDeleterServiceTests(ITestOutputHelper testOutputHelper)
        {
            //  _diagnosticContext
            _sut = new PersonDeleterService(_personRepository, _logger);
            _testOutputHelper = testOutputHelper;
        }

        #region DeletePerson


        [Fact]
        public async Task DeletePerson_ShouldThrowArgumentNullException_WhenPersonIdIsInvalid()
        {
            //  Arrange 
            int? PersonId = null;

            //  Act 
            var actionRequest = async () => await _sut.DeletePersonAsync(PersonId);

            // Assert 
            await actionRequest.Should().ThrowAsync<ArgumentNullException>();
        }

        [Fact]
        public async Task DeletePerson_ShouldReturnFalse_WhenPersonIdIsInvalid()
        {
            //  Arrange 
            int PersonId = 500;
            _personRepository.ExistsAsync(Arg.Any<int>()).Returns(false);

            //  Act 
            var actionRequest = await _sut.DeletePersonAsync(PersonId);

            // Assert 
            actionRequest.Should().BeFalse();
        }

        [Fact]
        public async Task DeletePerson_ShouldReturnTrue_WhenPersonIdIsValid()
        {

            //  Arrange 
            var personId = 1;

            _personRepository.ExistsAsync(Arg.Is<int>(x => x == 1)).Returns(true);
            _personRepository.DeletePersonAsync(Arg.Any<int>()).Returns(true);

            // Act 
            var res = await _sut.DeletePersonAsync(personId);

            // Assert 
            res.Should().BeTrue();
        }

        #endregion
    }
}

