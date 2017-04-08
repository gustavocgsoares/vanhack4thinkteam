using System;
using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class UnpredictableException : BaseException
    {
        #region Constructors | Destructors
        public UnpredictableException(Exception exception)
            : base(Messages.UnpredictableException)
        {
            Exception = exception;
        }
        #endregion

        #region Properties
        public Exception Exception { get; private set; }
        #endregion
    }
}
