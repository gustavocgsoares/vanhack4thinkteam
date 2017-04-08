using System;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.Extensions.Options;

namespace Farfetch.Data.Repositories.Corporate
{
    public class UserRepository
        : MongoDbRepository<User, Guid>, IUserRepository
    {
        #region Constructors | Destructors
        public UserRepository(IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "users")
        {
        }
        #endregion
    }
}
