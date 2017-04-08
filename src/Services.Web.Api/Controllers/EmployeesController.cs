using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Base;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/employees")]
    public class EmployeesController : BaseApiController
    {
        #region Fields | Members
        private readonly IEmployeeApp employeeApp;

        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="employeeApp"></param>
        /// <param name="urlHelperFactory"></param>
        public EmployeesController(
            IEmployeeApp employeeApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.employeeApp = employeeApp;
        }
        #endregion

        #region Static methods

        /// <summary>
        ///
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Link GetEmployeeByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var employeeLink = urlHelper
                .Link("GetEmployeeById", new { id = id });

            return new Link
            {
                Href = employeeLink,
                Method = Method.GET,
                Relations = self ? "self" : "employee_by_id"
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Link UpdateEmployeeLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var employeeLink = urlHelper
                .Link("UpdateEmployee", new { id = id });

            return new Link
            {
                Href = employeeLink,
                Method = Method.PUT,
                Relations = self ? "self" : "update_employee"
            };
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="urlHelper"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Link DeleteEmployeeLink(
            IUrlHelper urlHelper,
            string id)
        {
            var employeeLink = urlHelper
                .Link("DeleteEmployee", new { id = id });

            return new Link
            {
                Href = employeeLink,
                Method = Method.DELETE,
                Relations = "delete_employee"
            };
        }
        #endregion

        #region Services

        /// <summary>
        ///
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("", Name = "GetAllEmployees")]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 100)
        {
            var totalCount = 560;

            ////var pagedList = await _employeeApp.GetAllAsync(offset, limit);
            var pagedList = new PagedList<Employee>
            {
                Offset = offset,
                Limit = limit,
                Total = totalCount,
                Items = new List<Employee>()
            };

            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), Name = "John", Surname = "Smith Doe", Login = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), Name = "Jane", Surname = "Smith Doe", Login = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), Name = "Josh", Surname = "Smith Doe", Login = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), Name = "Alex", Surname = "Smith Doe", Login = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), Name = "Johnny", Surname = "Smith Doe", Login = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), Name = "Jude", Surname = "Smith Doe", Login = "conflict@domain.com" });

            var totalPages = (int)Math.Ceiling((double)totalCount / limit);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var prevLink = offset > 0 ? urlHelper.Link("GetAllEmployees", new { offset = limit > offset ? 0 : offset - limit, limit = limit }) : string.Empty;
            var nextLink = offset < totalCount - limit ? urlHelper.Link("GetAllEmployees", new { offset = offset + limit, limit = limit }) : string.Empty;

            var firstLink = offset > 0 ? urlHelper.Link("GetAllEmployees", new { offset = 0, limit = limit }) : string.Empty;
            var lastLink = offset < totalCount - limit ? urlHelper.Link("GetAllEmployees", new { offset = totalCount - limit, limit = limit }) : string.Empty;

            var links = new List<Link>();

            if (prevLink.HasValue())
            {
                links.Add(new Link { Href = prevLink, Method = Method.GET, Relations = "previous" });
            }

            if (nextLink.HasValue())
            {
                links.Add(new Link { Href = nextLink, Method = Method.GET, Relations = "next" });
            }

            if (firstLink.HasValue())
            {
                links.Add(new Link { Href = firstLink, Method = Method.GET, Relations = "first" });
            }

            if (lastLink.HasValue())
            {
                links.Add(new Link { Href = lastLink, Method = Method.GET, Relations = "last" });
            }

            var result = new EmployeesModel
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Links = links,
                Items = pagedList.Items.Select(e => EmployeeModel.ToModel(e)).ToList()
            };

            await Task.Run(() => result);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific employee by unique id
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Employee created</response>
        /// <response code="400">Employee has missing/invalid values</response>
        /// <response code="404">Employee not found or not exists</response>
        /// <response code="500">Oops! Can't get your employee right now</response>
        [HttpGet]
        [Route("{id}", Name = "GetEmployeeById")]
        [ProducesResponseType(typeof(EmployeeModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            ////var entity = await _employeeApp.GetAsync(id);

            var entity = new Employee
            {
                Id = Guid.NewGuid(),
                Name = "John Smith Doe",
                Login = "jsd@domain.com"
            };

            if (entity == null)
            {
                return NotFound();
            }

            await Task.Run(() => entity);
            var model = EmployeeModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetEmployeeByIdLink(urlHelper, model.Id, true);
            var deleteLink = DeleteEmployeeLink(urlHelper, model.Id);
            var updateLink = UpdateEmployeeLink(urlHelper, model.Id);

            model.Links = new List<Link>
            {
                getByIdlink,
                updateLink,
                deleteLink
            };

            return Ok(model);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("", Name = "CreateEmployee")]
        [Produces(typeof(EmployeeModel))]
        public async Task<IActionResult> Create([FromBody]EmployeeModel employee)
        {
            if (employee.Email == "conflict@domain.com")
            {
                return Conflict();
            }

            var result = new EmployeeModel
            {
                Id = "234rewr23422",
                FirstName = "John",
                LastName = "Smith Doe",
                Email = "conflict@domain.com"
            };

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetEmployeeByIdLink(urlHelper, result.Id);
            var deleteLink = DeleteEmployeeLink(urlHelper, result.Id);
            var updateLink = UpdateEmployeeLink(urlHelper, result.Id);

            result.Links = new List<Link>
            {
                getByIdlink,
                updateLink,
                deleteLink
            };

            await Task.Run(() => employee);
            return new CreatedResult(getByIdlink.Href, result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employee"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateEmployee")]
        [Produces(typeof(EmployeeModel))]
        public async Task<IActionResult> Update([FromUri] string id, [FromBody]EmployeeModel employee)
        {
            if (employee.FirstName == "conflict")
            {
                return Conflict();
            }
            else if (employee.FirstName == "notfound")
            {
                return NotFound();
            }

            var result = new EmployeeModel
            {
                Id = "234rewr23422",
                FirstName = "Jane",
                LastName = "Smith Doe",
                Email = "jsd@domain.com"
            };

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetEmployeeByIdLink(urlHelper, result.Id);
            var updateLink = UpdateEmployeeLink(urlHelper, result.Id, true);
            var deleteLink = DeleteEmployeeLink(urlHelper, result.Id);

            result.Links = new List<Link>
            {
                getByIdlink,
                updateLink,
                deleteLink
            };

            await Task.Run(() => employee);
            return Ok(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteEmployee")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == "notfound")
            {
                return NotFound();
            }

            await Task.Run(() => id);

            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion
    }
}
