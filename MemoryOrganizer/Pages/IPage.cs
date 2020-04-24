using System;

namespace MemoryOrganizer.Pages
{
    public interface IPage: ICloneable
    {
        Guid Id { get; }
    }
}
