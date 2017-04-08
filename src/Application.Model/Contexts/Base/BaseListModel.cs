using System.Collections;
using System.Collections.Generic;

namespace Farfetch.Application.Model.Contexts.Base
{
    public abstract class BaseListModel<TListModel, TModel> : List<TModel>
        where TListModel : ICollection, new()
    {
        #region Static methods
        public static TListModel Instance()
        {
            return new TListModel();
        }
        #endregion
    }
}