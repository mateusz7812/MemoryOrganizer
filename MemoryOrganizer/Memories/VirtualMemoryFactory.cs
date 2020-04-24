using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Memories
{
    class VirtualMemoryFactory: IVirtualMemoryFactory
    {
        public IVirtualMemory Create(List<IPage> pages) => new VirtualMemory(pages);
    }
}
