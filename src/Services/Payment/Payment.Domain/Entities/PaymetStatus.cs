using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Domain.Entities
{
    public enum PaymetStatus
    {
        Pending = 0,
        Completed = 1,
        Canceled = 2
    }
}
