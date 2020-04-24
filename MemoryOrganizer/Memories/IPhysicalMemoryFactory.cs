using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Frames;

namespace MemoryOrganizer.Memories
{
    public interface IPhysicalMemoryFactory
    {
        IPhysicalMemory Create(List<IFrame> frames);
    }
}
