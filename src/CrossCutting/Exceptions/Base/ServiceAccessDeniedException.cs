using Farfetch.CrossCutting.Resources.Exceptions.Base;

namespace Farfetch.CrossCutting.Exceptions.Base
{
    public class ServiceAccessDeniedException : BaseException
    {
        #region Constructors | Destructors
        public ServiceAccessDeniedException(string service)
            : base(Messages.ServiceAccessDeniedException, service)
        {
        }
        #endregion
    }
}
