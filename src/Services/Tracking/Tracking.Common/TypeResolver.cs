using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Tracking.Common
{
    public static class TypeResolver
    {
        public static List<Type> AssemblyTypes;

        static TypeResolver() {
            AssemblyTypes = Assembly.Load("Tracking.Domain")
                     .GetTypes().ToList();
        }

    }
}
