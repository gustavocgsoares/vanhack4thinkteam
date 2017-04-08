using System;
using Farfetch.Application.Contexts.Base;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Contexts.Corporate
{
    public class UserApp
        : BaseCrudApp<User, Guid>, IUserApp
    {
        #region Fields | Members
        private readonly IUserRepository userRepository;
        #endregion

        #region Constructors | Destructors
        public UserApp(IUserRepository userRepository)
            : base(userRepository)
        {
            this.userRepository = userRepository;
        }
        #endregion
    }
}
