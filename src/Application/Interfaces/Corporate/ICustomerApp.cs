using System;
using Farfetch.Application.Interfaces.Base;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Interfaces.Corporate
{
    public interface ICustomerApp : IBaseCrudApp<Customer, Guid>
    {
    }
}
