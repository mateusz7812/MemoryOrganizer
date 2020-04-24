using System;

namespace MemoryOrganizer.Frames
{
    public interface IFrameWithTimeStamps: IFrame
    {
        DateTime LastUsage { get; }
    }
}