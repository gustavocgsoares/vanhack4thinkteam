using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class KeyNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public KeyNotFoundException(string key)
            : base(Messages.KeyNotFoundException, key)
        {
        }
        #endregion
    }
}
