using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Enums.Base;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    /// Users APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/users")]
    public class UsersController : BaseApiController
    {
        #region Fields | Members

        /// <summary>
        /// User application flow.
        /// </summary>
        private readonly IUserApp userApp;

        /// <summary>
        /// See <see cref="IUrlHelperFactory"/>.
        /// </summary>
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UsersController"/> class.
        /// </summary>
        /// <param name="userApp">User application flow.</param>
        /// <param name="urlHelperFactory">See <see cref="IUrlHelperFactory"/>.</param>
        public UsersController(
            IUserApp userApp,
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
            this.userApp = userApp;
        }
        #endregion

        #region Static methods

        /// <summary>
        /// Get link to GetUserById API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link GetUserByIdLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var userLink = urlHelper
                .Link("GetUserById", new { id = id });

            return new Link
            {
                Href = userLink,
                Method = Method.GET,
                Relations = self ? "self" : "user_by_id"
            };
        }

        /// <summary>
        /// Get link to UpdateUser API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <param name="self">Indicate if is a self link.</param>
        /// <returns>API link.</returns>
        public static Link UpdateUserLink(
            IUrlHelper urlHelper,
            string id,
            bool self = false)
        {
            var userLink = urlHelper
                .Link("UpdateUser", new { id = id });

            return new Link
            {
                Href = userLink,
                Method = Method.PUT,
                Relations = self ? "self" : "update_user"
            };
        }

        /// <summary>
        /// Get link to DeleteUser API.
        /// </summary>
        /// <param name="urlHelper">Helper to build link.</param>
        /// <param name="id">User id.</param>
        /// <returns>API link.</returns>
        public static Link DeleteUserLink(
            IUrlHelper urlHelper,
            string id)
        {
            var userLink = urlHelper
                .Link("DeleteUser", new { id = id });

            return new Link
            {
                Href = userLink,
                Method = Method.DELETE,
                Relations = "delete_user"
            };
        }
        #endregion

        #region Services

        /// <summary>
        /// Retrieves a specific user by unique id.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <param name="id">User id.</param>
        /// <response code="200">User created. \o/</response>
        /// <response code="400">User has missing/invalid values.</response>
        /// <response code="404">User not found or not exists.</response>
        /// <response code="500">Oops! Can't get your user right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [Route("{id}", Name = "GetUserById")]
        [ProducesResponseType(typeof(UserModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> GetById(string id)
        {
            ////var entity = await _userApp.GetAsync(id);

            var entity = new User
            {
                Id = Guid.NewGuid(),
                Name = "John Smith Doe",
                Email = "jsd@domain.com",
                BirthDate = new DateTime(1988, 07, 03),
                Gender = Domain.Enums.Corporate.Gender.Male,
                ProfileImage = "../img/profiles/234rewr23422.png"
            };

            if (entity == null)
            {
                return NotFound();
            }

            await Task.Run(() => entity);
            var model = UserModel.ToModel(entity);

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);
            var getByIdlink = GetUserByIdLink(urlHelper, model.Id, true);
            var deleteLink = DeleteUserLink(urlHelper, model.Id);
            var updateLink = UpdateUserLink(urlHelper, model.Id);

            model.Links = new List<Link>
            {
                getByIdlink,
                updateLink,
                deleteLink
            };

            return Ok(model);
        }
        #endregion
    }
}
