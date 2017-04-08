using System;
using System.Net.Http;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Services.Web.Api.Tests.Base;
using Farfetch.Services.Web.Api.Tests.Controllers.Base;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.UpdateApi
{
    public class Configuration : BaseConfiguration
    {
        private ClassFixture fixture;

        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
            this.fixture = fixture;
        }

        public async Task<HttpResponseMessage> WhenRequestingTheUpdateEmployeeApiAsync(
            string employeeId = "",
            EmployeeModel employee = null)
        {
            return await RequestingApiAsync(new HttpMethod("PUT"), $"/v1/employees/{employeeId}", employee);
        }

        #region Givens
        public async Task<string> GivenAValidEmployeeId()
        {
            var createApiConfig = new CreateApi.Configuration(fixture);
            var employee = createApiConfig.GivenAValidEmployeeModel();
            employee.Email = "updateapi@domain.com";

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

        public EmployeeModel GivenAValidEmployeeModel()
        {
            return new EmployeeModel
            {
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "newupdateapi@domain.com",
                Active = true
            };
        }

        public EmployeeModel GivenAnEmptyEmployeeModel()
        {
            return new EmployeeModel();
        }

        public EmployeeModel GivenANullEmployeeModel()
        {
            return null;
        }
        #endregion
    }
}
