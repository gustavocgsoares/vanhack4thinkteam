using System;
using Farfetch.Application.Interfaces.Product;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Product;
using Microsoft.Extensions.Options;

namespace Farfetch.Data.Repositories.Product
{
    public class ItemRepository
        : MongoDbRepository<Item, Guid>, IItemRepository
    {
        #region Constructors | Destructors
        public ItemRepository(IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "items")
        {
        }
        #endregion
    }
}
