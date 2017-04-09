using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Sale;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Contexts.V1.Sale;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    /// Orders APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class OrdersController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// Order application flow.
        /// </summary>
        private readonly IOrderApp orderApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="orderApp">Order application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public OrdersController(
            IOrderApp orderApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.orderApp = orderApp;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetOrderById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetOrderByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var orderLink = urlHelper
                .Link("GetOrderById", new { id = id });

            return new Link
            {
                Href = orderLink,
                Method = Method.GET,
                Relations = self ? "self" : "order_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateOrder API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateOrderLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var orderLink = urlHelper
                .Link("UpdateOrder", new { id = id });

            return new Link
            {
                Href = orderLink,
                Method = Method.PUT,
                Relations = self ? "self" : "update_order"
            };
        }

        /// <summary>
        /// Get link to DeleteOrder API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteOrderLink(
            IUrlHelper urlHelper,
            string id)
        {
            var orderLink = urlHelper
                .Link("DeleteOrder", new { id = id });

            return new Link
            {
                Href = orderLink,
                Method = Method.DELETE,
                Relations = "delete_order"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Get all orders in a paged list.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="offset">Where to start returning records from the entire set of results. If you don't include this parameter, the default is to start at record number 0.</param>
        /// <param name="limit">How many records you want to return all at once. If you don't include this parameter, the limit is 100 records by default.</param>
        /// <response code="200">Orders found. \o/</response>
        /// <response code="400">Invalid values.</response>
        /// <response code="500">Oops! Can't get your orders right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("", Name = "GetAllOrders")]
        [ProducesResponseType(typeof(OrdersModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 100)
        {
            long totalCount = 0;
            var totalPages = 0;

            var pagedList = await orderApp.GetAllAsync(offset, limit);

            if (pagedList.IsNotNull() && pagedList.Items.IsNotNull() && pagedList.Items.Count > 0)
            {
                totalCount = pagedList.Total;
                totalPages = (int)Math.Ceiling((double)totalCount / limit);
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var prevLink = offset > 0 ? urlHelper.Link("GetAllOrders", new { offset = limit > offset ? 0 : offset - limit, limit = limit }) : string.Empty;
            var nextLink = offset < totalCount - limit ? urlHelper.Link("GetAllOrders", new { offset = offset + limit, limit = limit }) : string.Empty;

            var firstLink = offset > 0 ? urlHelper.Link("GetAllOrders", new { offset = 0, limit = limit }) : string.Empty;
            var lastLink = offset < totalCount - limit ? urlHelper.Link("GetAllOrders", new { offset = totalCount - limit, limit = limit }) : string.Empty;

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

            var result = new OrdersModel
            {
                TotalCount = pagedList.Total,
                TotalPages = totalPages,
                Links = links,
                Items = pagedList.Items.Select(e => OrderModel.ToModel(e)).ToList()
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific order by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">Order id.</param>
        /// <response code="200">Order found. \o/</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="404">Order not found or not exists.</response>
        /// <response code="500">Oops! Can't get your order right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetOrderById")]
        [ProducesResponseType(typeof(OrderModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var entity = await orderApp.GetAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                var model = OrderModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetOrderByIdLink(urlHelper, model.Id, true);
                var deleteLink = DeleteOrderLink(urlHelper, model.Id);
                var updateLink = UpdateOrderLink(urlHelper, model.Id);

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
        /// Create an order.
        /// </summary>
        /// <param name="order">Order to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Order created. \o/</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="409">Order has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your order right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateOrder")]
        [ProducesResponseType(typeof(OrderModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]OrderModel order)
        {
            try
            {
                order.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "order"));

                var entity = await orderApp.SaveAsync(order.ToDomain());
                var result = OrderModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetOrderByIdLink(urlHelper, result.Id);
                var deleteLink = DeleteOrderLink(urlHelper, result.Id);
                var updateLink = UpdateOrderLink(urlHelper, result.Id);

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
        /// Update an order.
        /// </summary>
        /// <param name="id">Order id to be updated.</param>
        /// <param name="order">Order to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Order updated. \o/</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="404">Order not found.</response>
        /// <response code="409">Order has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your order right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateOrder")]
        [ProducesResponseType(typeof(OrderModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]OrderModel order)
        {
            try
            {
                order.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "Order"));

                order.Id = id;

                var entity = await orderApp.SaveAsync(order.ToDomain());
                var result = OrderModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetOrderByIdLink(urlHelper, result.Id);
                var updateLink = UpdateOrderLink(urlHelper, result.Id, true);
                var deleteLink = DeleteOrderLink(urlHelper, result.Id);

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
        /// Delete an order.
        /// </summary>
        /// <param name="id">Order id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">Order deleted. o.O</response>
        /// <response code="400">Order has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your order right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteOrder")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await orderApp.DeleteAsync(id);
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
