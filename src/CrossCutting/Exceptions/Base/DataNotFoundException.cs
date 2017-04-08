using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class DataNotFoundException : BaseException
    {
        #region Constructors | Destructors
        public DataNotFoundException(string data)
            : base(Messages.KeyNotFoundException, data)
        {
        }
        #endregion
    }
}
