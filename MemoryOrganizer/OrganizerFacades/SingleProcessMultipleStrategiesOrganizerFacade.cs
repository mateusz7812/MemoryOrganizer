using System.Collections.Generic;
using MemoryOrganizer.Frames;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.Processes;
using MemoryOrganizer.Simulations;

namespace MemoryOrganizer.OrganizerFacades
{
    class SingleProcessMultipleStrategiesOrganizerFacade
    {
        private readonly ListPrototype<IPage> _pageListPrototype = new ListPrototype<IPage>();
        private readonly ListPrototype<IFrame> _frameListPrototype = new ListPrototype<IFrame>();

        private readonly ListPrototype<IPageReference> _pageReferencesListPrototype =
            new ListPrototype<IPageReference>();

        private readonly IPageListFactory _pageFactory = new PageListFactory();
        private readonly IPageReferenceListFactory _pageReferenceFactory = new PageReferenceListFactory();
        private readonly IFrameListFactory _frameFactory = new UniversalFrameFactory();
        private readonly IProcessFactory _processFactory = new ProcessFactory();
        public void GenerateNewPages(int amount)
        {
            _pageReferencesListPrototype.Set(null);
            _pageListPrototype.Set(_pageFactory.CreateList(amount));
        }

        public void GenerateNewFrames(int amount) => _frameListPrototype.Set(_frameFactory.CreateList(amount));

        public void GenerateNewPageReferences(int amount) =>
            _pageReferencesListPrototype.Set(_pageReferenceFactory.CreateList(_pageListPrototype.Clone(), amount));

        public void ProcessSimulationForStrategies(SingleProcessSimulation simulation, List<IStrategyFactory> strategyFactories)
        {
            foreach (var strategyFactory in strategyFactories)
            {
                simulation.PageList = _pageListPrototype.Clone();
                simulation.FrameList = _frameListPrototype.Clone();
                simulation.Process = _processFactory.Create(strategyFactory, _pageReferencesListPrototype.Clone());
                simulation.ProcessAll();
            }
        }
    }
}