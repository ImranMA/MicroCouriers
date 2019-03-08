using System;
using System.Collections.Generic;
using System.Text;

namespace ReadModel.AzFn.Events
{
    public class Aggregate
    {
        public string Id { get; set; }
        public int CurrentVersion { get; set; }
    }
}
