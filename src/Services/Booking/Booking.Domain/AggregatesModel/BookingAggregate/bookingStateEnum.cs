using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Domain.AggregatesModel.BookingAggregate
{
    public enum bookingStateEnum
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2       
    }
}
