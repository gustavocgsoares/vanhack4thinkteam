using System;
using Farfetch.Application.Interfaces.Sale;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Sale;
using Microsoft.Extensions.Options;

namespace Farfetch.Data.Repositories.Sale
{
    public class OrderRepository
        : MongoDbRepository<Order, Guid>, IOrderRepository
    {
        #region Constructors | Destructors
        public OrderRepository(IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "orders")
        {
        }
        #endregion
    }
}
