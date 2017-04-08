using System;
using System.Collections.Generic;
using System.Linq;
using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class BusinessConflictException : BaseException
    {
        #region Constructors | Destructors
        public BusinessConflictException(string message)
            : base(Messages.BusinessConflictException, message)
        {
        }

        public BusinessConflictException(IEnumerable<string> messages)
            : base(Messages.BusinessConflictException, string.Join(Environment.NewLine, (messages ?? new List<string>()).ToArray()))
        {
        }
        #endregion
    }
}
