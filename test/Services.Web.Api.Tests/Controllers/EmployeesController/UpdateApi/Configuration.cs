using System;
using System.Net.Http;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Enums.V1.Corporate;
using Farfetch.Services.Web.Api.Tests.Base;
using Farfetch.Services.Web.Api.Tests.Controllers.Base;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.UpdateApi
{
    public class Configuration : BaseConfiguration
    {
        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
        }

        public async Task<HttpResponseMessage> WhenRequestingTheUpdateEmployeeApiAsync(
            string employeeId = "",
            EmployeeModel employee = null)
        {
            return await RequestingApiAsync(new HttpMethod("PUT"), $"/v1/employees/{employeeId}", employee);
        }

        #region Givens
        public string GivenAValidEmployeeId()
        {
            return "wer23423";
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
                Email = "jsd@domain.com"
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
