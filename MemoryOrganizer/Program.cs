using System;
using System.Collections.Generic;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.Simulations;
using MemoryOrganizer.OrganizerFacades;

namespace MemoryOrganizer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var facade = new SingleProcessMultipleStrategiesOrganizerFacade();
            var strategyFactoryList = new List<IStrategyFactory>()
            {
                new FifoFactory(),
                new OptFactory(),
                new LruFactory(),
                new ALruFactory(),
                new RandomFactory()
            };
            var physicalMemoryFactory = new PhysicalMemoryFactory();
            var virtualMemoryFactory = new VirtualMemoryFactory();
            var simulation = new SingleProcessSimulation(physicalMemoryFactory, virtualMemoryFactory);
            facade.GenerateNewFrames(4);
            facade.GenerateNewPages(6);
            facade.GenerateNewPageReferences(10000);
            facade.ProcessSimulationForStrategies(simulation, strategyFactoryList);

            Console.ReadLine();
        }
    }
}