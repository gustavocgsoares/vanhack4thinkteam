using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Contexts.V1.Security;
using Farfetch.Application.Model.Enums.V1.Corporate;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/login")]
    public class LoginController : BaseApiController
    {
        #region Fields | Members
        private readonly IUrlHelperFactory urlHelperFactory;
        #endregion

        #region Constructors | Destructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="urlHelperFactory"></param>
        public LoginController(
            IUrlHelperFactory urlHelperFactory)
        {
            this.urlHelperFactory = urlHelperFactory;
        }
        #endregion

        #region Services

        /// <summary>
        ///
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{email}/{password}", Name = "Login")]
        [Produces(typeof(LoginModel))]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (email == "bloqued@domain.com")
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (email == "notfound@domain.com")
            {
                return NotFound();
            }

            var employee = new EmployeeModel
            {
                Id = "234rewr23422",
                FirstName = "John",
                LastName = "Smith Doe",
                Email = "jsd@domain.com"
            };

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var result = new LoginModel
            {
                Links = new List<Link>
                {
                    UsersController.GetUserByIdLink(urlHelper, "rwer3453erw")
                }
            };

            await Task.Run(() => employee);

            return Ok(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="socialNetwork"></param>
        /// <param name="externalCode"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{socialNetwork}/{externalCode}/social_network", Name = "LoginWithSocialNetwork")]
        [Produces(typeof(LoginModel))]
        public async Task<IActionResult> LoginWithSocialNetwork(SocialNetwork socialNetwork, string externalCode)
        {
            if (externalCode == "blocked")
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (externalCode == "notfound")
            {
                return NotFound();
            }

            var urlHelper = urlHelperFactory.GetUrlHelper(ControllerContext);

            var result = new LoginModel
            {
                Links = new List<Link>
                {
                    UsersController.GetUserByIdLink(urlHelper, "rwer3453erw")
                }
            };

            await Task.Run(() => result);
            return Ok(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{email}", Name = "RecoverPassword")]
        public async Task<IActionResult> RecoverPassword(string email)
        {
            if (email == "bloqued@domain.com")
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }
            else if (email == "notfound@domain.com")
            {
                return NotFound();
            }

            await Task.Run(() => email);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="pass"></param>
        /// <param name="newPass"></param>
        /// <param name="newPassConfirm"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{userId}/{pass}/{newPass}/{newPassConfirm}", Name = "ChangePassword")]
        public async Task<IActionResult> ChangePassword(
            [FromUri] string userId,
            [FromUri] string pass,
            [FromUri] string newPass,
            [FromUri] string newPassConfirm)
        {
            if (newPass != newPassConfirm)
            {
                return BadRequest();
            }
            else if (userId == "notfound")
            {
                return NotFound();
            }

            await Task.Run(() => userId);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="newPass"></param>
        /// <param name="newPassConfirm"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{newPass}/{newPassConfirm}/{token}", Name = "ChangePasswordByToken")]
        public async Task<IActionResult> ChangePasswordByToken(
            [FromUri] string newPass,
            [FromUri] string newPassConfirm,
            [FromUri] string token)
        {
            newPass.IsNullOrEmpty().Throw<InvalidParameterException>(string.Format(Messages.CannotBeNullOrEmpty, "newPass"));

            if (newPass != newPassConfirm)
            {
                return BadRequest();
            }
            else if (token == "notfound")
            {
                return NotFound();
            }

            await Task.Run(() => token);

            return StatusCode(HttpStatusCode.NoContent);
        }
        #endregion
    }
}
