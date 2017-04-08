using System.Net.Http;
using System.Threading.Tasks;
using Farfetch.Services.Web.Api.Tests.Base;
using Farfetch.Services.Web.Api.Tests.Controllers.Base;

namespace Farfetch.Services.Web.Api.Tests.Controllers.EmployeesController.GetByIdApi
{
    public class Configuration : BaseConfiguration
    {
        public Configuration(ClassFixture fixture)
            : base(fixture)
        {
        }

        public async Task<HttpResponseMessage> WhenRequestingTheGetEmployeeByIdApiAsync(string employeeId = "")
        {
            return await RequestingApiAsync(new HttpMethod("GET"), $"/v1/employees/{employeeId}");
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
        #endregion
    }
}
