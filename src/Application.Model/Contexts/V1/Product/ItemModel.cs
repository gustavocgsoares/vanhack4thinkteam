using System;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Product;

namespace Farfetch.Application.Model.Contexts.V1.Product
{
    public class ItemModel : BaseModel<ItemModel>
    {
        #region Properties
        public virtual string Id { get; set; }

        public virtual string Code { get; set; }

        public virtual string Name { get; set; }

        public virtual decimal Price { get; set; }

        public virtual int QuantityInStock { get; set; }

        public virtual string Description { get; set; }

        public virtual ItemAdditionalInfoModel AdditionalInfo { get; set; }

        public virtual bool? Active { get; set; }
        #endregion

        #region Converters
        public static ItemModel ToModel(Item entity, string url)
        {
            if (entity.IsNull())
            {
                return null;
            }

            var model = Instance();

            return ToModel(entity, model);
        }

        public static ItemModel ToModel(Item entity, ItemModel model = null)
        {
            if (entity.IsNull())
            {
                return null;
            }

            model = model ?? Instance();

            model.Id = entity.Id.ToString();
            model.Code = entity.Code;
            model.Name = entity.Name;
            model.Price = entity.Price;
            model.QuantityInStock = entity.QuantityInStock;
            model.Description = entity.Description;
            model.AdditionalInfo = ItemAdditionalInfoModel.ToModel(entity.AdditionalInfo);
            model.Active = entity.Active;

            return model;
        }

        public Item ToDomain()
        {
            return ToDomain(new Item());
        }

        public Item ToDomain(Item entity)
        {
            entity = entity ?? new Item();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : default(Guid);
            entity.Code = Code;
            entity.Name = Name;
            entity.Price = Price;
            entity.QuantityInStock = QuantityInStock;
            entity.Description = Description;
            entity.AdditionalInfo = AdditionalInfo.IsNotNull() ? AdditionalInfo.ToDomain() : null;
            entity.Active = Active.GetValueOrDefault();

            return entity;
        }
        #endregion
    }
}
