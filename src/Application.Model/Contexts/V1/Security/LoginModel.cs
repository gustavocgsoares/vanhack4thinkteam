using System.Collections.Generic;
using Farfetch.Application.Model.Contexts.Base;

namespace Farfetch.Application.Model.Contexts.V1.Security
{
    public class LoginModel : BaseModel<LoginModel>
    {
        #region Properties
        #endregion

        #region Converters
        public static LoginModel ToModel(List<Link> links = null)
        {
            var model = Instance();

            model.Links = links;

            return model;
        }
        #endregion
    }
}
