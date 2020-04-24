using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Frames;

namespace MemoryOrganizer.Memories
{
    public class PhysicalMemoryFactory: IPhysicalMemoryFactory
    {
        public IPhysicalMemory Create(List<IFrame> frames) => new PhysicalMemory(frames);
    }
}
