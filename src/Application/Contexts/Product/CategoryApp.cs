using System;
using Farfetch.Application.Contexts.Base;
using Farfetch.Application.Interfaces.Product;
using Farfetch.Domain.Entities.Product;

namespace Farfetch.Application.Contexts.Product
{
    public class CategoryApp
        : BaseCrudApp<Category, Guid>, ICategoryApp
    {
        #region Fields | Members
        private readonly ICategoryRepository categoryRepository;
        #endregion

        #region Constructors | Destructors
        public CategoryApp(ICategoryRepository categoryRepository)
            : base(categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        #endregion
    }
}
