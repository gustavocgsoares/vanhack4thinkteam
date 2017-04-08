using System.Net;
using System.Threading.Tasks;
using Farfetch.Services.Web.Api.Tests.Base;
using FluentAssertions;
using Xunit;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.DeleteApi
{
    [Collection("TestContext")]
    public class DeleteApiTests : Configuration
    {
        public DeleteApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "Employee Delete")]
        [Trait("Controller", "Employees")]
        [Fact]
        public async Task Should_return_success_with_right_data_sent()
        {
            ////Given
            var employeeId = GivenAValidEmployeeId();

            ////When
            var response = await WhenRequestingTheDeleteEmployeeApiAsync(employeeId);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }
    }
}
