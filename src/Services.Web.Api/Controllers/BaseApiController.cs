using System.Web.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farfetch.Services.Web.Api.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Authorize]
    [Controller]
    ////[RequireHttps]
    public abstract class BaseApiController : ApiController
    {
        #region Fields | Members

        /// <summary>
        ///
        /// </summary>
        public const string ApiDateFormat = ":datetime:regex(\\d{4}-\\d{2}-\\d{2})";

        /// <summary>
        ///
        /// </summary>
        public const string ApiDateTimeFormat = ":datetime:regex(\\d{4}-\\d{2}-\\d{2}T(\\d{2}:\\d{2}:\\d{2}|\\d{2}:\\d{2})";
        #endregion
    }
}
