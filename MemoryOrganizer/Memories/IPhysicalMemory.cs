using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.Memories
{
    public interface IPhysicalMemory: IMemory, IEnumerable<IFrame>
    {
        bool Contains(Guid pageId);
        IPhysicalMemory GetSubMemory(int amount);
        bool HaveFreeFrames { get; }
    }
}
