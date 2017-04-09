using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Product;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Contexts.V1.Product;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    /// Items APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ItemsController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// Item application flow.
        /// </summary>
        private readonly IItemApp itemApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsController"/> class.
        /// </summary>
        /// <param name="itemApp">Item application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public ItemsController(
            IItemApp itemApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.itemApp = itemApp;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetItemById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetItemByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var itemLink = urlHelper
                .Link("GetItemById", new { id = id });

            return new Link
            {
                Href = itemLink,
                Method = Method.GET,
                Relations = self ? "self" : "item_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateItem API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateItemLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var itemLink = urlHelper
                .Link("UpdateItem", new { id = id });

            return new Link
            {
                Href = itemLink,
                Method = Method.PUT,
                Relations = self ? "self" : "update_item"
            };
        }

        /// <summary>
        /// Get link to DeleteItem API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteItemLink(
            IUrlHelper urlHelper,
            string id)
        {
            var itemLink = urlHelper
                .Link("DeleteItem", new { id = id });

            return new Link
            {
                Href = itemLink,
                Method = Method.DELETE,
                Relations = "delete_item"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Get all items in a paged list.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="offset">Where to start returning records from the entire set of results. If you don't include this parameter, the default is to start at record number 0.</param>
        /// <param name="limit">How many records you want to return all at once. If you don't include this parameter, the limit is 100 records by default.</param>
        /// <response code="200">Items found. \o/</response>
        /// <response code="400">Invalid values.</response>
        /// <response code="500">Oops! Can't get your items right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("", Name = "GetAllItems")]
        [ProducesResponseType(typeof(ItemsModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 100)
        {
            long totalCount = 0;
            var totalPages = 0;

            var pagedList = await itemApp.GetAllAsync(offset, limit);

            if (pagedList.IsNotNull() && pagedList.Items.IsNotNull() && pagedList.Items.Count > 0)
            {
                totalCount = pagedList.Total;
                totalPages = (int)Math.Ceiling((double)totalCount / limit);
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var prevLink = offset > 0 ? urlHelper.Link("GetAllItems", new { offset = limit > offset ? 0 : offset - limit, limit = limit }) : string.Empty;
            var nextLink = offset < totalCount - limit ? urlHelper.Link("GetAllItems", new { offset = offset + limit, limit = limit }) : string.Empty;

            var firstLink = offset > 0 ? urlHelper.Link("GetAllItems", new { offset = 0, limit = limit }) : string.Empty;
            var lastLink = offset < totalCount - limit ? urlHelper.Link("GetAllItems", new { offset = totalCount - limit, limit = limit }) : string.Empty;

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

            var result = new ItemsModel
            {
                TotalCount = pagedList.Total,
                TotalPages = totalPages,
                Links = links,
                Items = pagedList.Items.Select(e => ItemModel.ToModel(e)).ToList()
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific item by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">Item id.</param>
        /// <response code="200">Item found. \o/</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="404">Item not found or not exists.</response>
        /// <response code="500">Oops! Can't get your item right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetItemById")]
        [ProducesResponseType(typeof(ItemModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var entity = await itemApp.GetAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                var model = ItemModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetItemByIdLink(urlHelper, model.Id, true);
                var deleteLink = DeleteItemLink(urlHelper, model.Id);
                var updateLink = UpdateItemLink(urlHelper, model.Id);

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
        /// Create an item.
        /// </summary>
        /// <param name="item">Item to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Item created. \o/</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="409">Item has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your item right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateItem")]
        [ProducesResponseType(typeof(ItemModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]ItemModel item)
        {
            try
            {
                item.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "item"));

                var entity = await itemApp.SaveAsync(item.ToDomain());
                var result = ItemModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetItemByIdLink(urlHelper, result.Id);
                var deleteLink = DeleteItemLink(urlHelper, result.Id);
                var updateLink = UpdateItemLink(urlHelper, result.Id);

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
        /// Update an item.
        /// </summary>
        /// <param name="id">Item id to be updated.</param>
        /// <param name="item">Item to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Item updated. \o/</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="404">Item not found.</response>
        /// <response code="409">Item has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your item right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateItem")]
        [ProducesResponseType(typeof(ItemModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]ItemModel item)
        {
            try
            {
                item.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "Item"));

                item.Id = id;

                var entity = await itemApp.SaveAsync(item.ToDomain());
                var result = ItemModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetItemByIdLink(urlHelper, result.Id);
                var updateLink = UpdateItemLink(urlHelper, result.Id, true);
                var deleteLink = DeleteItemLink(urlHelper, result.Id);

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
        /// Delete an item.
        /// </summary>
        /// <param name="id">Item id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">Item deleted. o.O</response>
        /// <response code="400">Item has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your item right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteItem")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await itemApp.DeleteAsync(id);
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
