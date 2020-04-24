using System;
using System.Collections.Generic;
using System.Text;

namespace MemoryOrganizer.Frames
{
    class FrameWithTimeStampsFactory : IFrameListFactory
    {
        public List<IFrame> CreateList(int amount)
        {
            var frames = new List<IFrame>();
            for (var i = 0; i < amount; i++)
                frames.Add(new FrameWithTimeStamps(Guid.NewGuid()));
            return frames;
        }
    }
}
