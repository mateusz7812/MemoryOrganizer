using System.Collections.Generic;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.Processes;

namespace MemoryOrganizer.Simulations
{
    public interface ISimulationTemplate
    {
        void ProcessAll();
        IPhysicalMemory PhysicalMemory { get; set; }
    }
}
