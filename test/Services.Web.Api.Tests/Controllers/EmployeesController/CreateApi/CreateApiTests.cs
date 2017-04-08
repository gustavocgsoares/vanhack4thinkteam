using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.Services.Web.Api.Tests.Base;
using FluentAssertions;
using Xunit;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.CreateApi
{
    [Collection("TestContext")]
    public class CreateApiTests : Configuration
    {
        public CreateApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "Employee Create")]
        [Trait("Controller", "Employees")]
        [Fact]
        public async Task Should_return_employee_created_with_right_data_sent()
        {
            ////Given
            var employee = GivenAValidEmployeeModel();

            ////When
            var response = await WhenRequestingTheCreateEmployeeApiAsync(employee);
            var result = await WhenGetContentAsModelAsync<EmployeeModel>(response);

            ////Then
            var lnkDefault = "http://localhost/v1/employees/";

            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            result.Should().NotBeNull();
            result.Links.Should().NotBeNullOrEmpty();
            result.Links.Should().HaveCount(3);

            response.Headers.Location.Should().NotBeNull();
            response.Headers.Location.ToString().Should().Be($"{lnkDefault}{result.Id}");

            var lnkEmployeeById = result.Links.FirstOrDefault(lnk => lnk.Relations == "employee_by_id");
            lnkEmployeeById.Should().NotBeNull();
            lnkEmployeeById.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkEmployeeById.Method.Should().Be(Method.GET);

            var lnkEmployeeDelete = result.Links.FirstOrDefault(lnk => lnk.Relations == "delete_employee");
            lnkEmployeeDelete.Should().NotBeNull();
            lnkEmployeeDelete.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkEmployeeDelete.Method.Should().Be(Method.DELETE);

            var lnkEmployeeUpdate = result.Links.FirstOrDefault(lnk => lnk.Relations == "update_employee");
            lnkEmployeeUpdate.Should().NotBeNull();
            lnkEmployeeUpdate.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkEmployeeUpdate.Method.Should().Be(Method.PUT);
        }

        [Trait("CI", "")]
        [Trait("Api", "Employee Create")]
        [Trait("Controller", "Employees")]
        [Fact]
        public async Task Should_return_error_when_employee_email_already_exists()
        {
            ////Given
            var employee = GivenAnEmployeeModelWithConflictingEmail();

            ////When
            var response = await WhenRequestingTheCreateEmployeeApiAsync(employee);

            ////Then
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.Conflict);
        }
    }
}
