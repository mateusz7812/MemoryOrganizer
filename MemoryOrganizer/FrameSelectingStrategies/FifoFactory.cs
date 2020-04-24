using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Memories;
using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizingStrategies
{
    class FifoFactory: IStrategyFactory
    {
        public IOrganizingStrategy Create(IProcess processor) => 
            new FifoStrategy(processor.PhysicalMemory);
    }
}
