using System;
using Farfetch.Application.Contexts.Base;
using Farfetch.Application.Interfaces.Product;
using Farfetch.Domain.Entities.Product;

namespace Farfetch.Application.Contexts.Product
{
    public class ItemApp
        : BaseCrudApp<Item, Guid>, IItemApp
    {
        #region Fields | Members
        private readonly IItemRepository itemRepository;
        #endregion

        #region Constructors | Destructors
        public ItemApp(IItemRepository itemRepository)
            : base(itemRepository)
        {
            this.itemRepository = itemRepository;
        }
        #endregion
    }
}
