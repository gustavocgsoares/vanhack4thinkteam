using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.CrossCutting.Exceptions.Base;
using Farfetch.CrossCutting.ExtensionMethods;
using Farfetch.CrossCutting.Resources.Validations;
using Farfetch.Domain.Entities.Base;

namespace Farfetch.Domain.Entities.Product
{
    public class Category : Entity<Category, Guid>
    {
        #region Constructors | Destructors
        public Category()
        {
        }
        #endregion

        #region Properties
        public virtual string Name { get; set; }

        public virtual string Description { get; set; }

        public virtual string ImageUrl { get; set; }
        #endregion

        #region Entity members
        public override void ValidateProperties(Enums.Base.Action action)
        {
            throw new NotImplementedException();
        }

        public override async Task AddAsync(IRepository<Category, Guid> repository)
        {
            var category = await repository.GetFirstAsync(e => e.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase));
            category.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "name"));

            Active = true;
            AddedDate = DateTime.UtcNow;

            await repository.SaveAsync(this);
        }

        public override async Task UpdateAsync(IRepository<Category, Guid> repository)
        {
            var category = await repository.GetAsync(Id);
            category.IsNull().Throw<DataNotFoundException>(Id);

            if (Name.HasValue() && !category.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase))
            {
                var categorySameEmail = await repository.GetFirstAsync(e => e.Name.Equals(Name, StringComparison.CurrentCultureIgnoreCase));
                categorySameEmail.IsNotNull().Throw<BusinessConflictException>(string.Format(Messages.AlreadyExists, "name"));

                category.Name = Name;
            }

            if (Name.HasValue())
            {
                category.Name = Name;
            }

            if (Description.HasValue())
            {
                category.Description = Description;
            }

            if (ImageUrl.HasValue())
            {
                category.ImageUrl = ImageUrl;
            }

            category.Active = Active;
            ModifiedDate = DateTime.UtcNow;

            await repository.SaveAsync(category);
        }

        public override async Task DeleteAsync(IRepository<Category, Guid> repository)
        {
            var category = await repository.GetAsync(Id);

            if (category.IsNotNull())
            {
                Active = false;
                DeletedDate = DateTime.UtcNow;
                await repository.SaveAsync(this);
            }
        }
        #endregion
    }
}
