﻿using System;
using Farfetch.Application.Interfaces.Base;
using Farfetch.Domain.Entities.Sale;

namespace Farfetch.Application.Interfaces.Sale
{
    public interface IOrderApp : IBaseCrudApp<Order, Guid>
    {
    }
}
