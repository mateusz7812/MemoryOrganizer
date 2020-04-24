using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Memories
{
    public interface IVirtualMemoryFactory
    {
        IVirtualMemory Create(List<IPage> pages);
    }
}
