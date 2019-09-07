using Shipping.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Domain.Interfaces
{
    public interface IShippingRepository
    {
        Task<Shippings> AddShippingAsync(Shippings shippings);
        Task<Shippings> AddShippingHisotryAsync(ShippingsHistory shippingsHistory);
    }
}
