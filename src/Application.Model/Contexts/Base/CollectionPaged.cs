using System.Collections.Generic;
using Newtonsoft.Json;

namespace Farfetch.Application.Model.Contexts.Base
{
    public abstract class CollectionPaged<TListModel> : Resource
        where TListModel : new()
    {
        public virtual long TotalCount { get; set; }

        public virtual int TotalPages { get; set; }

        public virtual List<TListModel> Items { get; set; }

        #region Static methods
        public static TListModel Instance()
        {
            return new TListModel();
        }
        #endregion
    }
}
