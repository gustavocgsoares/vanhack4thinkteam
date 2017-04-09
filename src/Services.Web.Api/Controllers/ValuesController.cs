using System.Collections.Generic;
using System.Threading.Tasks;
using Farfetch.Application.Model.Contexts.V1.Security;
using Microsoft.AspNetCore.Mvc;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    /// Employees APIs.
    /// </summary>
    [ApiVersion("1")]
    [Route("v{version:apiVersion}/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Login by social network like facebook, google plus, among others.
        /// </summary>
        /// <remarks>Awesomeness!</remarks>
        /// <response code="200">Authenticated user. \o/</response>
        /// <response code="400">Invalid parameter values.</response>
        /// <response code="403">Blocked login.</response>
        /// <response code="404">Login not found or not exists.</response>
        /// <response code="500">Oops! Can't get your authentication right now.</response>
        /// <returns>Any status code and response as described.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(LoginModel), 200)]
        [ProducesResponseType(typeof(IDictionary<string, string>), 400)]
        [ProducesResponseType(typeof(void), 403)]
        [ProducesResponseType(typeof(void), 404)]
        [ProducesResponseType(typeof(void), 500)]
        public async Task<IActionResult> Get()
        {
            await Task.Run(() => new string[] { "value1", "value2" });
            return Ok(new string[] { "value1", "value2" });
        }

        /// <summary>
        /// Get one.
        /// </summary>
        /// <param name="id">User id.</param>
        /// <returns>Return value.</returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Post value.
        /// </summary>
        /// <param name="value">Value to post.</param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Put value.
        /// </summary>
        /// <param name="id">Id value.</param>
        /// <param name="value">String value.</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Delete value.
        /// </summary>
        /// <param name="id">Id to be deleted.</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
