using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.Services.Web.Api.Tests.Base;
using FluentAssertions;
using Xunit;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.GetByIdApi
{
    [Collection("TestContext")]
    public class GetByIdApiTests : Configuration
    {
        public GetByIdApiTests(ClassFixture fixture)
            : base(fixture)
        {
        }

        [Trait("CI", "")]
        [Trait("Api", "Employee Get by Id")]
        [Trait("Controller", "Employees")]
        [Fact]
        public async Task Should_return_the_employee_data_found()
        {
            ////Given
            var employeeId = await GivenAValidEmployeeId();

            ////When
            var response = await WhenRequestingTheGetEmployeeByIdApiAsync(employeeId);
            var result = await WhenGetContentAsModelAsync<EmployeeModel>(response);

            ////Then
            response.IsSuccessStatusCode.Should().BeTrue();
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            result.Should().NotBeNull();
            result.Links.Should().NotBeNullOrEmpty();
            result.Links.Should().HaveCount(3);

            var lnkDefault = "http://localhost/v1/employees/";

            var lnkEmployeeById = result.Links.FirstOrDefault(lnk => lnk.Relations == "self");
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
    }
}
