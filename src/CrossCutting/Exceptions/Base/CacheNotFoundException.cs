using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class CacheNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public CacheNotFoundException(string cacheName)
            : base(Messages.CacheNotFoundException, cacheName)
        {
        }
        #endregion
    }
}
