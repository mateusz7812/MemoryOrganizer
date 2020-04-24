using System;
using System.Collections.Generic;
using System.Text;
using MemoryOrganizer.Memories;
using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizingStrategies
{
    public interface IStrategyFactory
    {
        IOrganizingStrategy Create(IProcess processor);
    }
}
