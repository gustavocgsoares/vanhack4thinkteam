using System;
using System.Net.Http;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Services.Web.Api.Tests.Base;
using Farfetch.Services.Web.Api.Tests.Controllers.Base;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.CreateApi
{
    public class Configuration : BaseConfiguration
    {
        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
        }

        public async Task<HttpResponseMessage> WhenRequestingTheCreateEmployeeApiAsync(EmployeeModel employee = null)
        {
            return await RequestingApiAsync(new HttpMethod("POST"), $"/v1/employees", employee);
        }

        #region Givens
        public EmployeeModel GivenAValidEmployeeModel()
        {
            return new EmployeeModel
            {
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "jsd@domain.com"
            };
        }

        public EmployeeModel GivenAnEmployeeModelWithConflictingEmail()
        {
            return new EmployeeModel
            {
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "conflict@domain.com"
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
