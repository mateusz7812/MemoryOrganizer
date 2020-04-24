using System.Collections.Generic;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.Processes
{
    public interface IProcess
    {
        IPhysicalMemory PhysicalMemory { get; set; }
        IVirtualMemory VirtualMemory { get; set; }
        int PagesErrors { get; }
        string StrategyName { get; }
        bool HasNextPageToLoad { get; }
        List<IPageReference> PageReferenceList { get; }
        IOrganizingStrategy Strategy { get; set; }
        void LoadNextPage();
    }
}