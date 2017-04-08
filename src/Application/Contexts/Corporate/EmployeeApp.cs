using System;
using Farfetch.Application.Contexts.Base;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Contexts.Corporate
{
    public class EmployeeApp
        : BaseCrudApp<Employee, Guid>, IEmployeeApp
    {
        #region Fields | Members
        private readonly IEmployeeRepository employeeRepository;
        #endregion

        #region Constructors | Destructors
        public EmployeeApp(IEmployeeRepository employeeRepository)
            : base(employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }
        #endregion
    }
}
