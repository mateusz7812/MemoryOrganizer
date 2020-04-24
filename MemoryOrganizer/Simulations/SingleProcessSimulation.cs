using System;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.Processes;

namespace MemoryOrganizer.Simulations
{
    public class SingleProcessSimulation : ISimulationTemplate
    {
        private readonly IPhysicalMemoryFactory _physicalMemoryFactory;
        private readonly IVirtualMemoryFactory _virtualMemoryFactory;
        
        public IPhysicalMemory PhysicalMemory { get; set; }

        private IPhysicalMemory _physicalMemory;
        private IVirtualMemory _virtualMemory;
        public string StrategyName { get; private set; }
        public int PageErrors { get; private set; }

        public List<IFrame> FrameList { get; set; }
        public List<IPageReference> PageReferenceList { get; set; }
        
        public List<IPage> PageList { get; set; }
        public IProcess Process { get; set; }

        public SingleProcessSimulation(IPhysicalMemoryFactory physicalMemoryFactory,
            IVirtualMemoryFactory virtualMemoryFactory)
        {
            _physicalMemoryFactory = physicalMemoryFactory;
            _virtualMemoryFactory = virtualMemoryFactory;
        }

        public void ProcessAll()
        {
            _physicalMemory = _physicalMemoryFactory.Create(FrameList);
            _virtualMemory = _virtualMemoryFactory.Create(PageList);
            
            Process.PhysicalMemory = _physicalMemory;
            Process.VirtualMemory = _virtualMemory;

            while (Process.HasNextPageToLoad)
                Process.LoadNextPage();

            StrategyName = Process.ToString();
            PageErrors = Process.PagesErrors;
        }

    }
}