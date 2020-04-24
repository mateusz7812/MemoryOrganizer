using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizingStrategies
{
    public class RandomFactory: IStrategyFactory
    {
        public IOrganizingStrategy Create(IProcess processor)
        {
            return new RandomStrategy(processor.PhysicalMemory);
        }
    }
}