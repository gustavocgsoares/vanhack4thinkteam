using System;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Data.MongoDb.Repositories.Base;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Data.Repositories.Corporate
{
    public class EmployeeRepository
        : MongoDbRepository<Employee, Guid>, IEmployeeRepository
    {
        #region Constructors | Destructors
        public EmployeeRepository()
            : base("employees")
        {
        }
        #endregion
    }
}
