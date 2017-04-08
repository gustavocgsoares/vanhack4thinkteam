using System;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Corporate;
using Microsoft.Extensions.Options;

namespace Farfetch.Data.Repositories.Corporate
{
    public class EmployeeRepository
        : MongoDbRepository<Employee, Guid>, IEmployeeRepository
    {
        #region Constructors | Destructors
        public EmployeeRepository(IOptions<CrossCutting.Configurations.Data> data)
            : base(data, "employees")
        {
        }
        #endregion
    }
}
