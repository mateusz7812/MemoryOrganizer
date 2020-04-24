using System;
using MemoryOrganizer.Pages;

namespace MemoryOrganizer.PageReferences
{
    public interface IPageReference: ICloneable
    {
        Guid PageId { get; }
    }
}
