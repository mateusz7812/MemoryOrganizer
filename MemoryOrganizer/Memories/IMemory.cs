using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer
{
    public interface IMemory
    {
        void Accept(IReplacingVisitor visitor);
    }
}
