using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class ALruFactory: IStrategyFactory
    {
        public IOrganizingStrategy Create(IProcess processor) => 
            new ALruStrategy(processor.PhysicalMemory);
    }
}