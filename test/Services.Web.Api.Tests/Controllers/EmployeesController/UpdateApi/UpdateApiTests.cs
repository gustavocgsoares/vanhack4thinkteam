using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.Services.Web.Api.Tests.Base;
using FluentAssertions;
using Xunit;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.UpdateApi
{
    [Collection("TestContext")]
    public class UpdateApiTests : Configuration
    {
        public UpdateApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "Employee Update")]
        [Trait("Controller", "Employees")]
        [Fact]
        public async Task Should_return_success_with_right_data_sent()
        {
            ////Given
            var employeeId = await GivenAValidEmployeeId();
            var employee = GivenAValidEmployeeModel();

            ////When
            var response = await WhenRequestingTheUpdateEmployeeApiAsync(employeeId, employee);
            var result = await WhenGetContentAsModelAsync<EmployeeModel>(response);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
            result.Links.Should().NotBeNullOrEmpty();
            result.Links.Should().HaveCount(3);

            var lnkDefault = "http://localhost/v1/employees/";

            var lnkEmployeeById = result.Links.FirstOrDefault(lnk => lnk.Relations == "employee_by_id");
            lnkEmployeeById.Should().NotBeNull();
            lnkEmployeeById.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkEmployeeById.Method.Should().Be(Method.GET);

            var lnkEmployeeDelete = result.Links.FirstOrDefault(lnk => lnk.Relations == "delete_employee");
            lnkEmployeeDelete.Should().NotBeNull();
            lnkEmployeeDelete.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkEmployeeDelete.Method.Should().Be(Method.DELETE);

            var lnkEmployeeUpdate = result.Links.FirstOrDefault(lnk => lnk.Relations == "self");
            lnkEmployeeUpdate.Should().NotBeNull();
            lnkEmployeeUpdate.Href.Should().StartWith($"{lnkDefault}{result.Id}");
            lnkEmployeeUpdate.Method.Should().Be(Method.PUT);
        }
    }
}
