using System;
using System.Threading.Tasks;
using Farfetch.Application.Interfaces.Base;
using Farfetch.Domain.Entities.Corporate;

namespace Farfetch.Application.Interfaces.Corporate
{
    public interface ICustomerRepository : IRepository<Customer, Guid>
    {
    }
}
