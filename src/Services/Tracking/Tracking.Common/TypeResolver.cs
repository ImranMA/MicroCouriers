using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tracking.Common
{
    public static class TypeResolver
    {
        //We keep the reference of all the events in List
        public static List<Type> AssemblyTypes;

        //Load and save all the event types dynamically
        static TypeResolver() {
            AssemblyTypes = Assembly.Load("Tracking.Domain")
                     .GetTypes().ToList();
        }

    }
}
