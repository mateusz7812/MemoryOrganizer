using System.Collections.Generic;

namespace MemoryOrganizer.Frames
{
    interface IFrameListFactory
    {
        List<IFrame> CreateList(int amount);
    }
}
