using System;
using System.Collections.Generic;
using System.Text;

namespace Tracking.Domain.Interfaces
{
    public interface ICahce
    {
        bool Exists(string key);
        void Save(string key, string value);
        string Get(string key);
        void Remove(string key);
        void Clear();
    }
}
