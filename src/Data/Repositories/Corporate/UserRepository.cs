using System;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Data.Repositories.Corporate
{
    public class UserRepository
        : MongoDbRepository<User, Guid>, IUserRepository
    {
        #region Constructors | Destructors
        public UserRepository()
            : base("users")
        {
        }
        #endregion
    }
}
