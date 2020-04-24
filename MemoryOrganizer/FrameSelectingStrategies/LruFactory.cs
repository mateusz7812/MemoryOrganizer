using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class LruFactory: IStrategyFactory
    {
        public IOrganizingStrategy Create(IProcess processor) => 
            new LruStrategy(processor.PhysicalMemory);
    }
}