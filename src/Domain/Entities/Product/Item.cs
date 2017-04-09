using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Farfetch.Domain.Entities.Base;
using Farfetch.Domain.Entities.Product.Aggregates.Item;

namespace Farfetch.Domain.Entities.Product
{
    public class Item : Entity<Item, Guid>
    {
        #region Constructors | Destructors
        public Item()
        {
        }
        #endregion

        #region Properties
        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int QuantityInStock { get; set; }

        public virtual string Description { get; set; }

        public virtual ItemAdditionalInfo AdditionalInfo { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Item, Guid> repository)
        {
            var item = await repository.GetFirstAsync(e => e.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase));
            item.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "code"));

            Active = true;
            AddedDate = DateTime.UtcNow;

            await repository.SaveAsync(this);
        }

        public override async Task UpdateAsync(IRepository<Item, Guid> repository)
        {
            var item = await repository.GetAsync(Id);
            item.IsNull().Throw<DataNotFoundException>(Id);

            if (Code.HasValue() && !item.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase))
            {
                var itemSameCode = await repository.GetFirstAsync(e => e.Code.Equals(Code, StringComparison.CurrentCultureIgnoreCase));
                itemSameCode.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "code"));

                item.Code = Code;
            }

            if (Name.HasValue())
            {
                item.Name = Name;
            }

            if (Price > 0)
            {
                item.Price = Price;
            }

            if (AdditionalInfo.IsNotNull())
            {
                AdditionalInfo.UpdateProperties(item.AdditionalInfo);
            }

            item.Active = Active;
            ModifiedDate = DateTime.UtcNow;

            await repository.SaveAsync(item);
        }

        public override async Task DeleteAsync(IRepository<Item, Guid> repository)
        {
            var item = await repository.GetAsync(Id);

            if (item.IsNotNull())
            {
                Active = false;
                DeletedDate = DateTime.UtcNow;
                await repository.SaveAsync(this);
            }
        }
        #endregion
    }
}
