using System;
using System.Collections.Generic;
using System.Linq;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.Application.Model.Contexts.V1.Corporate;
using Farfetch.Application.Model.Contexts.V1.Product;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Sale;

namespace Farfetch.Application.Model.Contexts.V1.Sale
{
    public class OrderModel : BaseModel<OrderModel>
    {
        #region Properties
        public virtual string Id { get; set; }

        public virtual CustomerModel Customer { get; set; }

        public virtual List<ItemModel> Items { get; set; }

        public virtual DateTime CreationDate { get; set; }

        public virtual bool? Active { get; set; }
        #endregion

        #region Converters
        public static OrderModel ToModel(Order entity)
        {
            if (entity.IsNull())
            {
                return null;
            }

            var model = Instance();

            return ToModel(entity, model);
        }

        public static OrderModel ToModel(Order entity, OrderModel model = null)
        {
            if (entity.IsNull())
            {
                return null;
            }

            model = model ?? Instance();

            model.Id = entity.Id.ToString();
            model.CreationDate = entity.AddedDate;
            model.Customer = CustomerModel.ToModel(entity.Customer);
            model.Items = entity.Items.IsNotNull() && entity.Items.Count > 0 ? entity.Items.Select(i => ItemModel.ToModel(i)).ToList() : null;
            model.Active = entity.Active;

            return model;
        }

        public Order ToDomain()
        {
            return ToDomain(new Order());
        }

        public Order ToDomain(Order entity)
        {
            entity = entity ?? new Order();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : default(Guid);
            entity.AddedDate = CreationDate;
            entity.Customer = Customer.IsNotNull() ? Customer.ToDomain() : null;
            entity.Items = Items.IsNotNull() && Items.Count > 0 ? Items.Select(i => i.ToDomain()).ToList() : null;
            entity.Active = Active.GetValueOrDefault();

            return entity;
        }
        #endregion
    }
}
