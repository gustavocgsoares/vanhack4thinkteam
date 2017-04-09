using System;
using Farfetch.Application.Model.Contexts.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.Domain.Entities.Product;

namespace Farfetch.Application.Model.Contexts.V1.Product
{
    public class CategoryModel : BaseModel<CategoryModel>
    {
        #region Properties
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ImageUrl { get; set; }

        public virtual bool? Active { get; set; }
        #endregion

        #region Converters
        public static CategoryModel ToModel(Category entity)
        {
            if (entity.IsNull())
            {
                return null;
            }

            var model = Instance();

            return ToModel(entity, model);
        }

        public static CategoryModel ToModel(Category entity, CategoryModel model = null)
        {
            if (entity.IsNull())
            {
                return null;
            }

            model = model ?? Instance();

            model.Id = entity.Id.ToString();
            model.Name = entity.Name;
            model.Description = entity.Description;
            model.ImageUrl = entity.ImageUrl;
            model.Active = entity.Active;

            return model;
        }

        public Category ToDomain()
        {
            return ToDomain(new Category());
        }

        public Category ToDomain(Category entity)
        {
            entity = entity ?? new Category();

            entity.Id = Id.HasValue() ? Id.To<Guid>() : default(Guid);
            entity.Name = Name;
            entity.Description = Description;
            entity.ImageUrl = ImageUrl;
            entity.Active = Active.GetValueOrDefault();

            return entity;
        }
        #endregion
    }
}
