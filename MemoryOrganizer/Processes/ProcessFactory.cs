using System.Collections.Generic;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;

namespace MemoryOrganizer.Processes
{
    class ProcessFactory: IProcessFactory
    {
        public IProcess Create(IStrategyFactory strategyFactory, List<IPageReference> clone)
        {
            throw new System.NotImplementedException();
        }
    }
}
