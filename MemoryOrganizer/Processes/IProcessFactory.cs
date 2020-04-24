using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;

namespace MemoryOrganizer.Processes
{
    interface IProcessFactory
    {
        IProcess Create(IStrategyFactory strategyFactory, List<IPageReference> clone);
    }
}
