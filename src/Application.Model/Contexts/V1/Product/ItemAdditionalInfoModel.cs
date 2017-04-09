using Farfetch.Application.Model.Contexts.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Product.Aggregates.Item;

namespace Farfetch.Application.Model.Contexts.V1.Product
{
    public class ItemAdditionalInfoModel : BaseModel<ItemAdditionalInfoModel>
    {
        #region Properties
        public virtual decimal? Weight { get; set; }

        public virtual string Material { get; set; }

        public virtual string Colors { get; set; }

        public virtual string Brand { get; set; }
        #endregion

        #region Converters
        public static ItemAdditionalInfoModel ToModel(ItemAdditionalInfo entity)
        {
            if (entity.IsNull())
            {
                return null;
            }

            var model = Instance();

            return ToModel(entity, model);
        }

        public static ItemAdditionalInfoModel ToModel(ItemAdditionalInfo entity, ItemAdditionalInfoModel model = null)
        {
            if (entity.IsNull())
            {
                return null;
            }

            model = model ?? Instance();

            model.Weight = entity.Weight > 0 ? entity.Weight : default(decimal?);
            model.Material = entity.Material;
            model.Colors = entity.Colors;
            model.Brand = entity.Brand;

            return model;
        }

        public ItemAdditionalInfo ToDomain()
        {
            return ToDomain(new ItemAdditionalInfo());
        }

        public ItemAdditionalInfo ToDomain(ItemAdditionalInfo entity)
        {
            entity = entity ?? new ItemAdditionalInfo();

            entity.Weight = Weight.GetValueOrDefault();
            entity.Material = Material;
            entity.Colors = Colors;
            entity.Brand = Brand;

            return entity;
        }
        #endregion
    }
}
