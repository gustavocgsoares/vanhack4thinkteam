using System;
using System.Collections.Generic;
using System.Linq;
using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class InvalidParameterException : BaseException
    {
        #region Constructors | Destructors
        public InvalidParameterException(string error)
            : base(Messages.InvalidParameterException, error)
        {
        }

        public InvalidParameterException(IEnumerable<string> errors)
            : base(Messages.InvalidParameterException, string.Join(Environment.NewLine, (errors ?? new List<string>()).ToArray()))
        {
        }
        #endregion
    }
}