using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class CacheValueNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public CacheValueNotFoundException(string value)
            : base(Messages.CacheValueNotFoundException, value)
        {
        }
        #endregion
    }
}
