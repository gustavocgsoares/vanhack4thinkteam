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
    /// Categories APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class CategoriesController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// Category application flow.
        /// </summary>
        private readonly ICategoryApp categoryApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoriesController"/> class.
        /// </summary>
        /// <param name="categoryApp">Category application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public CategoriesController(
            ICategoryApp categoryApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.categoryApp = categoryApp;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetCategoryById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetCategoryByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var categoryLink = urlHelper
                .Link("GetCategoryById", new { id = id });

            return new Link
            {
                Href = categoryLink,
                Method = Method.GET,
                Relations = self ? "self" : "category_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateCategory API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateCategoryLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var categoryLink = urlHelper
                .Link("UpdateCategory", new { id = id });

            return new Link
            {
                Href = categoryLink,
                Method = Method.PUT,
                Relations = self ? "self" : "update_category"
            };
        }

        /// <summary>
        /// Get link to DeleteCategory API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteCategoryLink(
            IUrlHelper urlHelper,
            string id)
        {
            var categoryLink = urlHelper
                .Link("DeleteCategory", new { id = id });

            return new Link
            {
                Href = categoryLink,
                Method = Method.DELETE,
                Relations = "delete_category"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Get all categories in a paged list.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="offset">Where to start returning records from the entire set of results. If you don't include this parameter, the default is to start at record number 0.</param>
        /// <param name="limit">How many records you want to return all at once. If you don't include this parameter, the limit is 100 records by default.</param>
        /// <response code="200">Categories found. \o/</response>
        /// <response code="400">Invalid values.</response>
        /// <response code="500">Oops! Can't get your categories right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("", Name = "GetAllCategories")]
        [ProducesResponseType(typeof(CategoriesModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetAll(int offset = 0, int limit = 100)
        {
            long totalCount = 0;
            var totalPages = 0;

            var pagedList = await categoryApp.GetAllAsync(offset, limit);

            if (pagedList.IsNotNull() && pagedList.Items.IsNotNull() && pagedList.Items.Count > 0)
            {
                totalCount = pagedList.Total;
                totalPages = (int)Math.Ceiling((double)totalCount / limit);
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var prevLink = offset > 0 ? urlHelper.Link("GetAllCategories", new { offset = limit > offset ? 0 : offset - limit, limit = limit }) : string.Empty;
            var nextLink = offset < totalCount - limit ? urlHelper.Link("GetAllCategories", new { offset = offset + limit, limit = limit }) : string.Empty;

            var firstLink = offset > 0 ? urlHelper.Link("GetAllCategories", new { offset = 0, limit = limit }) : string.Empty;
            var lastLink = offset < totalCount - limit ? urlHelper.Link("GetAllCategories", new { offset = totalCount - limit, limit = limit }) : string.Empty;

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

            var result = new CategoriesModel
            {
                TotalCount = pagedList.Total,
                TotalPages = totalPages,
                Links = links,
                Items = pagedList.Items.Select(e => CategoryModel.ToModel(e)).ToList()
            };

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a specific category by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">Category id.</param>
        /// <response code="200">Category found. \o/</response>
        /// <response code="400">Category has missing/invalid values.</response>
        /// <response code="404">Category not found or not exists.</response>
        /// <response code="500">Oops! Can't get your category right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetCategoryById")]
        [ProducesResponseType(typeof(CategoryModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            try
            {
                var entity = await categoryApp.GetAsync(id);

                if (entity.IsNull())
                {
                    return NotFound();
                }

                var model = CategoryModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetCategoryByIdLink(urlHelper, model.Id, true);
                var deleteLink = DeleteCategoryLink(urlHelper, model.Id);
                var updateLink = UpdateCategoryLink(urlHelper, model.Id);

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
        /// Create an category.
        /// </summary>
        /// <param name="category">Category to be created.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="201">Category created. \o/</response>
        /// <response code="400">Category has missing/invalid values.</response>
        /// <response code="409">Category has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't create your category right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPost]
        [Route("", Name = "CreateCategory")]
        [ProducesResponseType(typeof(CategoryModel), 201)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Create([FromBody]CategoryModel category)
        {
            try
            {
                category.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "category"));

                var entity = await categoryApp.SaveAsync(category.ToDomain());
                var result = CategoryModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetCategoryByIdLink(urlHelper, result.Id);
                var deleteLink = DeleteCategoryLink(urlHelper, result.Id);
                var updateLink = UpdateCategoryLink(urlHelper, result.Id);

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
        /// Update an category.
        /// </summary>
        /// <param name="id">Category id to be updated.</param>
        /// <param name="category">Category to be updated.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Category updated. \o/</response>
        /// <response code="400">Category has missing/invalid values.</response>
        /// <response code="404">Category not found.</response>
        /// <response code="409">Category has conflicting values with existing data. Eg: Email.</response>
        /// <response code="500">Oops! Can't update your category right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpPut]
        [Route("{id}", Name = "UpdateCategory")]
        [ProducesResponseType(typeof(CategoryModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 409)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Update(string id, [FromBody]CategoryModel category)
        {
            try
            {
                category.IsNull().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNull, "Category"));

                category.Id = id;

                var entity = await categoryApp.SaveAsync(category.ToDomain());
                var result = CategoryModel.ToModel(entity);

                var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
                var getByIdlink = GetCategoryByIdLink(urlHelper, result.Id);
                var updateLink = UpdateCategoryLink(urlHelper, result.Id, true);
                var deleteLink = DeleteCategoryLink(urlHelper, result.Id);

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
        /// Delete an category.
        /// </summary>
        /// <param name="id">Category id to be deleted.</param>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="204">Category deleted. o.O</response>
        /// <response code="400">Category has missing/invalid values.</response>
        /// <response code="500">Oops! Can't create your category right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpDelete]
        [Route("{id}", Name = "DeleteCategory")]
        [ProducesResponseType(typeof(void), 204)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await categoryApp.DeleteAsync(id);
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
