using System;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Base;

namespace Farfetch.Domain.Entities.Product.Aggregates.Item
{
    public class ItemAdditionalInfo : Entity<ItemAdditionalInfo, Guid>
    {
        #region Constructors | Destructors
        public ItemAdditionalInfo()
        {
        }
        #endregion

        #region Properties
        public virtual decimal Weight { get; set; }

        public virtual string Material { get; set; }

        public virtual string Colors { get; set; }

        public virtual string Brand { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        internal void UpdateProperties(ItemAdditionalInfo additionalInfo)
        {
            if (Weight > 0)
            {
                additionalInfo.Weight = Weight;
            }

            if (Material.HasValue())
            {
                additionalInfo.Material = Material;
            }

            if (Colors.HasValue())
            {
                additionalInfo.Colors = Colors;
            }

            if (Brand.HasValue())
            {
                additionalInfo.Brand = Brand;
            }
        }
        #endregion
    }
}
