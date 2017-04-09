using System;
using Farfetch.Application.Interfaces.Product;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Product;
using Microsoft.Extensions.Options;

namespace Farfetch.Data.Repositories.Product
{
    public class CategoryRepository
        : MongoDbRepository<Category, Guid>, ICategoryRepository
    {
        #region Constructors | Destructors
        public CategoryRepository(IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "categories")
        {
        }
        #endregion
    }
}
