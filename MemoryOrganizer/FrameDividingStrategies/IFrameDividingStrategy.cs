using System.Collections.Generic;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.Processes;

namespace MemoryOrganizer.FrameDividingStrategies
{
    public interface IFrameDividingStrategy
    {
        IPhysicalMemory GetPhysicalMemoryForProcess(IProcess process);
    }
}