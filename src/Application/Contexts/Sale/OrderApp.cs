using System;
using Farfetch.Application.Contexts.Base;
using Farfetch.Application.Interfaces.Sale;
using Farfetch.Domain.Entities.Sale;

namespace Farfetch.Application.Contexts.Sale
{
    public class OrderApp
        : BaseCrudApp<Order, Guid>, IOrderApp
    {
        #region Fields | Members
        private readonly IOrderRepository orderRepository;
        #endregion

        #region Constructors | Destructors
        public OrderApp(IOrderRepository orderRepository)
            : base(orderRepository)
        {
            this.orderRepository = orderRepository;
        }
        #endregion
    }
}
