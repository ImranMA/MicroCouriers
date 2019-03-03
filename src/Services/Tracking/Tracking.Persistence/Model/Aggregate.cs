using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Persistence.Model
{
    public class Aggregate
    {
        public string Id { get; set; }
        public int CurrentVersion { get; set; }
    }
}
