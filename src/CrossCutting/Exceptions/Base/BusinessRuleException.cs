using System;
using System.Collections.Generic;
using System.Linq;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class BusinessRuleException : BaseException
    {
        #region Constructors | Destructors
        public BusinessRuleException(string message)
            : base(Resources.Exceptions.Base.Messages.BusinessRuleException, message)
        {
            Messages = new List<string>();
            Messages.Add(message);
        }

        public BusinessRuleException(string message, params string[] args)
            : base(string.Format(Resources.Exceptions.Base.Messages.BusinessRuleException, message), args)
        {
            Messages.Add(message);
        }

        public BusinessRuleException(IEnumerable<string> messages)
            : base(Resources.Exceptions.Base.Messages.BusinessRuleException, string.Join(Environment.NewLine, (messages ?? new List<string>()).ToArray()))
        {
            Messages.AddRange(messages);
        }
        #endregion

        #region Properties
        public List<string> Messages { get; private set; }
        #endregion
    }
}
