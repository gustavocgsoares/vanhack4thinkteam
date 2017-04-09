using System;
using Farfetch.Application.Contexts.Base;
using Farfetch.Application.Interfaces.Corporate;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Contexts.Corporate
{
    public class CustomerApp
        : BaseCrudApp<Customer, Guid>, ICustomerApp
    {
        #region Fields | Members
        private readonly ICustomerRepository customerRepository;
        #endregion

        #region Constructors | Destructors
        public CustomerApp(ICustomerRepository customerRepository)
            : base(customerRepository)
        {
            this.customerRepository = customerRepository;
        }
        #endregion
    }
}
