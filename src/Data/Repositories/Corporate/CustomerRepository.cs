using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.Extensions.Options;

namespace Farfetch.Data.Repositories.Corporate
{
    public class CustomerRepository
        : MongoDbRepository<Customer, Guid>, ICustomerRepository
    {
        #region Constructors | Destructors
        public CustomerRepository(IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "customers")
        {
        }
        #endregion
    }
}
