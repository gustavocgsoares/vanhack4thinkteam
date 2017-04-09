using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Base;
using Farfetch.Domain.Entities.Corporate;
using Farfetch.Domain.Entities.Product;

namespace Farfetch.Domain.Entities.Sale
{
    public class Order : Entity<Order, Guid>
    {
        #region Constructors | Destructors
        public Order()
        {
        }
        #endregion

        #region Properties
        public virtual Customer Customer { get; set; }

        public virtual List<Item> Items { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Order, Guid> repository)
        {
            Active = true;
            AddedDate = DateTime.UtcNow;

            await repository.SaveAsync(this);
        }

        public override async Task DeleteAsync(IRepository<Order, Guid> repository)
        {
            var order = await repository.GetAsync(Id);

            if (order.IsNotNull())
            {
                Active = false;
                DeletedDate = DateTime.UtcNow;
                await repository.SaveAsync(this);
            }
        }
        #endregion
    }
}
