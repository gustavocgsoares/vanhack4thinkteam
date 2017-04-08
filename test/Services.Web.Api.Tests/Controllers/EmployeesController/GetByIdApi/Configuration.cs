using System.Net.Http;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Services.Web.Api.Tests.Base;
using Farfetch.Services.Web.Api.Tests.Controllers.Base;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.GetByIdApi
{
    public class Configuration : BaseConfiguration
    {
        private ClassFixture fixture;

        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
            this.fixture = fixture;
        }

        public async Task<HttpResponseMessage> WhenRequestingTheGetEmployeeByIdApiAsync(string employeeId = "")
        {
            return await RequestingApiAsync(new HttpMethod("GET"), $"/v1/employees/{employeeId}");
        }

        #region Givens
        public async Task<string> GivenAValidEmployeeId()
        {
            var createApiConfig = new CreateApi.Configuration(fixture);
            var employee = createApiConfig.GivenAValidEmployeeModel();
            employee.Email = "getbyidapi@domain.com";

            var response = await createApiConfig.WhenRequestingTheCreateEmployeeApiAsync(employee);
            var result = await createApiConfig.WhenGetContentAsModelAsync<EmployeeModel>(response);

            return result.Id;
        }

        public string GivenANotExistsEmployeeId()
        {
            return "notfound";
        }

        public string GivenAnEmptyEmployeeId()
        {
            return string.Empty;
        }
        #endregion
    }
}
