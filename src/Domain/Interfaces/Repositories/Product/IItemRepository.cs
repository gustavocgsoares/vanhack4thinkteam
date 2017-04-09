using System;
using Farfetch.Application.Interfaces.Base;
using Farfetch.Domain.Entities.Product;

namespace Farfetch.Application.Interfaces.Product
{
    public interface IItemRepository : IRepository<Item, Guid>
    {
    }
}
