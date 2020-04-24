using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MemoryOrganizer.FrameDividingStrategies;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.Processes;

namespace MemoryOrganizer.Simulations
{
    public class MultiProcessSimulation: ISimulationTemplate
    {
        public void ProcessAll()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<IPageReference> AllPagesReferences => Processes.SelectMany(p => p.PageReferenceList);

        public MultiProcessSimulation()
        {
            _processesEnum = GetProcessesEnumerable().GetEnumerator();
        }
        
        public IPhysicalMemory PhysicalMemory { get; set; }
        public List<IProcess> Processes { get; } = new List<IProcess>();
        private readonly IEnumerator<IProcess> _processesEnum;
        public IFrameDividingStrategy FrameDivideStrategy { get; set; }

        private IEnumerable<IProcess> GetProcessesEnumerable()
        {
            while (true)
            {
                foreach (var process in Processes)
                {
                    yield return process;
                }
            }
        }
        
        public void ProcessOne()
        {
            _processesEnum.MoveNext();
            var process = _processesEnum.Current;
            if(process == null) throw new NullReferenceException("no more processes");
            if (PhysicalMemory.HaveFreeFrames)
            {
                var physicalMemoryOfProcess = FrameDivideStrategy.GetPhysicalMemoryForProcess(process);
                process.PhysicalMemory = physicalMemoryOfProcess;
            }
            process.LoadNextPage();
        }
    }
}
