using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizingStrategies
{
    class OptFactory: IStrategyFactory
    {
        public IOrganizingStrategy Create(IProcess processor) => 
            new OptStrategy(processor.PhysicalMemory, processor.PageReferenceList);
    }
}
