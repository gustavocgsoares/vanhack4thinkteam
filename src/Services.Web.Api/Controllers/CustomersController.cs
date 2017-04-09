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
    /// Customers APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CustomersController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// Customer application flow.
        /// </summary>
        private readonly ICustomerApp customerApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomersController"/> class.
        /// </summary>
        /// <param name="customerApp">Customer application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public CustomersController(
            ICustomerApp customerApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.customerApp = customerApp;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetCustomerById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetCustomerByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var customerLink = urlHelper
                .Link("GetCustomerById", new { id = id });

            return new Link
            {
                Href = customerLink,
                Method = Method.GET,
                Relations = self ? "self" : "customer_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateCustomer API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateCustomerLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var customerLink = urlHelper
                .Link("UpdateCustomer", new { id = id });

            return new Link
            {
                Href = customerLink,
                Method = Method.PUT,
                Relations = self ? "self" : "update_customer"
            };
        }

        /// <summary>
        /// Get link to DeleteCustomer API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteCustomerLink(
            IUrlHelper urlHelper,
            string id)
        {
            var customerLink = urlHelper
                .Link("DeleteCustomer", new { id = id });

            return new Link
            {
                Href = customerLink,
                Method = Method.DELETE,
                Relations = "delete_customer"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Retrieves a specific customer by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">Customer id.</param>
        /// <response code="200">Customer found. \o/</response>
        /// <response code="400">Customer has missing/invalid values.</response>
        /// <response code="404">Customer not found or not exists.</response>
        /// <response code="500">Oops! Can't get your customer right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetCustomerById")]
        [ProducesResponseType(typeof(CustomerModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var entity = await customerApp.GetAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                var model = CustomerModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetCustomerByIdLink(urlHelper, model.Id, true);
                var deleteLink = DeleteCustomerLink(urlHelper, model.Id);
                var updateLink = UpdateCustomerLink(urlHelper, model.Id);

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
        /// Create an customer.
        /// </summary>
        /// <param name="customer">Customer to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Customer created. \o/</response>
        /// <response code="400">Customer has missing/invalid values.</response>
        /// <response code="409">Customer has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your customer right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateCustomer")]
        [ProducesResponseType(typeof(CustomerModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]CustomerModel customer)
        {
            try
            {
                customer.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "customer"));

                var entity = await customerApp.SaveAsync(customer.ToDomain());
                var result = CustomerModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetCustomerByIdLink(urlHelper, result.Id);
                var deleteLink = DeleteCustomerLink(urlHelper, result.Id);
                var updateLink = UpdateCustomerLink(urlHelper, result.Id);

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
        /// Update an customer.
        /// </summary>
        /// <param name="id">Customer id to be updated.</param>
        /// <param name="customer">Customer to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Customer updated. \o/</response>
        /// <response code="400">Customer has missing/invalid values.</response>
        /// <response code="404">Customer not found.</response>
        /// <response code="409">Customer has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your customer right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateCustomer")]
        [ProducesResponseType(typeof(CustomerModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]CustomerModel customer)
        {
            try
            {
                customer.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "Customer"));

                customer.Id = id;

                var entity = await customerApp.SaveAsync(customer.ToDomain());
                var result = CustomerModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetCustomerByIdLink(urlHelper, result.Id);
                var updateLink = UpdateCustomerLink(urlHelper, result.Id, true);
                var deleteLink = DeleteCustomerLink(urlHelper, result.Id);

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
        /// Delete an customer.
        /// </summary>
        /// <param name="id">Customer id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">Customer deleted. o.O</response>
        /// <response code="400">Customer has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your customer right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteCustomer")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await customerApp.DeleteAsync(id);
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
