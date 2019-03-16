using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroCourier.Web.DTO
{
    public class BookingOrderDetailDTO
    {
        public string PackageType { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
