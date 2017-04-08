using System;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public abstract class BaseException : Exception
    {
        #region Constructors | Destructors
        public BaseException(string message, params string[] args)
            : base(string.Format(message, args))
        {
            ErrorCode = GetType().Name;
        }
        #endregion

        #region Properties
        public string ErrorCode { get; private set; }
        #endregion
    }
}
