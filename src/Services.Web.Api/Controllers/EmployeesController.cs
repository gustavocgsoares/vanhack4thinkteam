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
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Farfetch.Domain.Entities.Base;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    /// Employees APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/employees")]
    public class EmployeesController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// Employee application flow.
        /// </summary>
        private readonly IEmployeeApp employeeApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="employeeApp">Employee application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
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
        /// Get link to GetEmployeeById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
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
        /// Get link to UpdateEmployee API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
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
        /// Get link to DeleteEmployee API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
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
        /// Get all employees in a paged list.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="offset">Where to start returning records from the entire set of results. If you don't include this parameter, the default is to start at record number 0.</param>
        /// <param name="limit">How many records you want to return all at once. If you don't include this parameter, the limit is 100 records by default.</param>
        /// <response code="200">Employees found. \o/</response>
        /// <response code="400">Invalid values.</response>
        /// <response code="500">Oops! Can't get your employees right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("", Name = "GetAllEmployees")]
        [ProducesResponseType(typeof(EmployeeModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
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

            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), FirstName = "John", LastName = "Smith Doe", Email = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), FirstName = "Jane", LastName = "Smith Doe", Email = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), FirstName = "Josh", LastName = "Smith Doe", Email = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), FirstName = "Alex", LastName = "Smith Doe", Email = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), FirstName = "Johnny", LastName = "Smith Doe", Email = "conflict@domain.com" });
            pagedList.Items.Add(new Employee { Id = Guid.NewGuid(), FirstName = "Jude", LastName = "Smith Doe", Email = "conflict@domain.com" });

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
        /// Retrieves a specific employee by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">Employee id.</param>
        /// <response code="200">Employee found. \o/</response>
        /// <response code="400">Employee has missing/invalid values.</response>
        /// <response code="404">Employee not found or not exists.</response>
        /// <response code="500">Oops! Can't get your employee right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetEmployeeById")]
        [ProducesResponseType(typeof(EmployeeModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var entity = await employeeApp.GetAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

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
            catch (InvalidParameterException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new UnpredictableException(ex));
            }
        }

        /// <summary>
        /// Create an employee.
        /// </summary>
        /// <param name="employee">Employee to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Employee created. \o/</response>
        /// <response code="400">Employee has missing/invalid values.</response>
        /// <response code="409">Employee has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your employee right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateEmployee")]
        [ProducesResponseType(typeof(EmployeeModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]EmployeeModel employee)
        {
            try
            {
                employee.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "employee"));

                var entity = await employeeApp.SaveAsync(employee.ToDomain());
                var result = EmployeeModel.ToModel(entity);

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

                return new CreatedResult(getByIdlink.Href, result);
            }
            catch (InvalidParameterException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (BusinessConflictException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return InternalServerError(new UnpredictableException(ex));
            }
        }

        /// <summary>
        /// Update an employee.
        /// </summary>
        /// <param name="id">Employee id to be updated.</param>
        /// <param name="employee">Employee to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Employee updated. \o/</response>
        /// <response code="400">Employee has missing/invalid values.</response>
        /// <response code="404">Employee not found.</response>
        /// <response code="409">Employee has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your employee right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateEmployee")]
        [ProducesResponseType(typeof(EmployeeModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]EmployeeModel employee)
        {
            try
            {
                employee.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "employee"));

                employee.Id = id;

                var entity = await employeeApp.SaveAsync(employee.ToDomain());
                var result = EmployeeModel.ToModel(entity);

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

                return Ok(result);
            }
            catch (InvalidParameterException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (DataNotFoundException)
            {
                return NotFound();
            }
            catch (BusinessConflictException)
            {
                return Conflict();
            }
            catch (Exception ex)
            {
                return InternalServerError(new UnpredictableException(ex));
            }
        }

        /// <summary>
        /// Delete an employee.
        /// </summary>
        /// <param name="id">Employee id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">Employee deleted. o.O</response>
        /// <response code="400">Employee has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your employee right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteEmployee")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await employeeApp.DeleteAsync(id);
                return StatusCode(HttpStatusCode.NoContent);
            }
            catch (InvalidParameterException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return InternalServerError(new UnpredictableException(ex));
            }
        }
        #endregion
    }
}
