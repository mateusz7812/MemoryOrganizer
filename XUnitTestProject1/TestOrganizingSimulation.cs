using System;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer;
using MemoryOrganizer.FrameDividingStrategies;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.Pages;
using MemoryOrganizer.Processes;
using MemoryOrganizer.ReplacingVisitors;
using MemoryOrganizer.Simulations;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class TestOrganizingSimulation
    {
        [Fact]
        public void TestSingleProcessSimulation()
        {

            var pages = new List<IPage>
            {
                Mock.Of<IPage>(p => p.Id == Guid.NewGuid()),
                Mock.Of<IPage>(p => p.Id == Guid.NewGuid()),
                Mock.Of<IPage>(p => p.Id == Guid.NewGuid()),
                Mock.Of<IPage>(p => p.Id == Guid.NewGuid()),
            };

            var frames = new List<IFrame>()
            {
                Mock.Of<IFrame>(f => f.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(f => f.Id == Guid.NewGuid()),
            };

            var pageReferences = new List<IPageReference>()
            {
                Mock.Of<IPageReference>(r => r.PageId == pages[0].Id),
                Mock.Of<IPageReference>(r => r.PageId == pages[1].Id),
                Mock.Of<IPageReference>(r => r.PageId == pages[1].Id),
                Mock.Of<IPageReference>(r => r.PageId == pages[2].Id),
                Mock.Of<IPageReference>(r => r.PageId == pages[1].Id),
                Mock.Of<IPageReference>(r => r.PageId == pages[3].Id),
                Mock.Of<IPageReference>(r => r.PageId == pages[1].Id),
            };

            var nextReplacedFrameIndex = 0;

            var strategy = Mock.Of<IOrganizingStrategy>();
            Mock.Get(strategy).Setup(m => m.CreateReplacingVisitor(It.IsAny<IPageReference>()))
                .Returns((IPageReference pageReference) =>
                {
                    var visitor = Mock.Of<IReplacingVisitor>(m =>
                        m.PageId == pageReference.PageId &&
                        m.FrameId == frames[nextReplacedFrameIndex].Id);
                    nextReplacedFrameIndex = 1 - nextReplacedFrameIndex;
                    return visitor;
                });

            var pagesReferencesIterator = pageReferences.GetEnumerator();
            pagesReferencesIterator.MoveNext();

            var process = Mock.Of<IProcess>();
            Mock.Get(process).SetupAllProperties();
            Mock.Get(process).Setup(m => m.HasNextPageToLoad).Returns(
                () => pagesReferencesIterator.Current != null);
            Mock.Get(process).Setup(m => m.LoadNextPage()).Callback(() =>
            {
                var pageReference = pagesReferencesIterator.Current;
                var replacing = strategy.CreateReplacingVisitor(pageReference);
                if (replacing == null) throw new NullReferenceException("no more pages to load");
                process.VirtualMemory.Accept(replacing);
                process.PhysicalMemory.Accept(replacing);
                pagesReferencesIterator.MoveNext();
            });

            void ReplacePages(IReplacingVisitor visitor) => frames.First(f => f.Id.Equals(visitor.FrameId)).Page = visitor.Page;

            var virtualMemory = Mock.Of<IVirtualMemory>();
            Mock.Get(virtualMemory).Setup(m => m.Accept(It.IsAny<IReplacingVisitor>())).Callback((IReplacingVisitor visitor) => visitor.Page = pages.First(p => p.Id.Equals(visitor.PageId)));

            var virtualMemoryFactory = Mock.Of<IVirtualMemoryFactory>();
            Mock.Get(virtualMemoryFactory).Setup(m => m.Create(It.IsAny<List<IPage>>())).Returns(virtualMemory);

            var physicalMemory = Mock.Of<IPhysicalMemory>();
            Mock.Get(physicalMemory).Setup(m => m.Accept(It.IsAny<IReplacingVisitor>())).Callback((IReplacingVisitor visitor) => ReplacePages(visitor));
            Mock.Get(physicalMemory).Setup(m => m.Contains(It.IsAny<Guid>())).Returns<Guid>((p) => frames.Where(f => f.Page != null).Count(f => f.Page.Id.Equals(p)) == 1);

            var physicalMemoryFactory = Mock.Of<IPhysicalMemoryFactory>();
            Mock.Get(physicalMemoryFactory).Setup(m => m.Create(It.IsAny<List<IFrame>>())).Returns(physicalMemory);

            var simulation = new SingleProcessSimulation(physicalMemoryFactory, virtualMemoryFactory)
            {
                PageReferenceList = pageReferences,
                Process = process
            };

            simulation.ProcessAll();

            Mock.Get(physicalMemory).Verify(m => m.Accept(It.IsAny<IReplacingVisitor>()), Times.Exactly(7));
        }

        [Fact]
        public void TestMultiProcessesSimulation()
        {
            var pages = new List<IPage>
            {
                Mock.Of<IPage>(p => p.Id == Guid.NewGuid()),
                Mock.Of<IPage>(p => p.Id == Guid.NewGuid()),
            };

            var frames = new List<IFrame>()
            {
                Mock.Of<IFrame>(f => f.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(f => f.Id == Guid.NewGuid()),
            };

            var pageReferences1 = new List<IPageReference>()
            {
                Mock.Of<IPageReference>(r => r.PageId == pages[0].Id)
            };
            var pagesReferences1Iterator = pageReferences1.GetEnumerator();
            pagesReferences1Iterator.MoveNext();

            var pageReferences2 = new List<IPageReference>(){
                Mock.Of<IPageReference>(r => r.PageId == pages[1].Id)
            };
            var pagesReferences2Iterator = pageReferences2.GetEnumerator();
            pagesReferences2Iterator.MoveNext();
            
            void LoadNextPageMock(IEnumerator<IPageReference> pageReferencesEnumerator, IProcess process)
            {
                var pageReference = pageReferencesEnumerator.Current;
                var replacing = process.Strategy.CreateReplacingVisitor(pageReference);
                if (replacing == null) throw new NullReferenceException("no more pages to load");
                process.VirtualMemory.Accept(replacing);
                process.PhysicalMemory.Accept(replacing);
                pageReferencesEnumerator.MoveNext();
            }

            IProcess ProcessMock(List<IPageReference>.Enumerator pagesReferencesIterator, IOrganizingStrategy strategy)
            {
                var process = Mock.Of<IProcess>();
                Mock.Get(process).SetupAllProperties();
                process.Strategy = strategy;
                var virtualMemory = Mock.Of<IVirtualMemory>();
                Mock.Get(virtualMemory).Setup(m => m.Accept(It.IsAny<IReplacingVisitor>()))
                    .Callback<IReplacingVisitor>((visitor) =>
                        visitor.Page = pages.Find(p => p.Id.Equals(visitor.PageId)));
                process.VirtualMemory = virtualMemory;
                Mock.Get(process).Setup(m => m.HasNextPageToLoad).Returns(() => pagesReferencesIterator.Current != null);
                Mock.Get(process).Setup(m => m.LoadNextPage()).Callback(() => { LoadNextPageMock(pagesReferencesIterator, process); });
                return process;
            }

            IOrganizingStrategy StrategyMock(IFrame frame)
            {
                var strategy = Mock.Of<IOrganizingStrategy>();
                Mock.Get(strategy).Setup(m => m.CreateReplacingVisitor(It.IsAny<IPageReference>()))
                    .Returns<IPageReference>(
                        (pref) =>
                        {
                            return Mock.Of<IReplacingVisitor>(m =>
                                m.PageId == pref.PageId &&
                                m.Page == pages.Find(p => p.Id.Equals(pref.PageId)) &&
                                m.FrameId == frame.Id);
                        });
                return strategy;
            }

            var frame1 = frames[0];
            var frame2 = frames[1];

            var strategy1 = StrategyMock(frame1);
            var strategy2 = StrategyMock(frame2);
            
            var process1 = ProcessMock(pagesReferences1Iterator, strategy1);
            var process2 = ProcessMock(pagesReferences2Iterator, strategy2);

            var framesStrategy = Mock.Of<IFrameDividingStrategy>();
            Mock.Get(framesStrategy).Setup(m => m.GetPhysicalMemoryForProcess(process1)).Returns(() => PhysicalMemoryMock(frame1));
            Mock.Get(framesStrategy).Setup(m => m.GetPhysicalMemoryForProcess(process2)).Returns(() => PhysicalMemoryMock(frame2));

            IPhysicalMemory PhysicalMemoryMock(IFrame frame)
            {
                var frameList = new List<IFrame>() {frame};
                var memory = Mock.Of<IPhysicalMemory>();
                Mock.Get(memory).Setup(m => m.Contains(It.IsAny<Guid>()))
                    .Returns<Guid>((g) => frameList.Any(m => m.HavePage(g)));
                Mock.Get(memory).Setup(m => m.GetEnumerator()).Returns(() => frameList.GetEnumerator());
                Mock.Get(memory).Setup(m => m.Accept(It.IsAny<IReplacingVisitor>()))
                    .Callback<IReplacingVisitor>((visitor =>
                        frames.Find(f => f.Id.Equals(visitor.FrameId)).Page = visitor.Page));
                return memory;
            }

            var mainPhysicalMemory = Mock.Of<IPhysicalMemory>();
            Mock.Get(mainPhysicalMemory).Setup(m => m.GetEnumerator()).Returns(() => frames.GetEnumerator());
            Mock.Get(mainPhysicalMemory).Setup(m => m.HaveFreeFrames).Returns(true);
            
            var simulation = new MultiProcessSimulation();
            simulation.Processes.Add(process1);
            simulation.Processes.Add(process2);
            simulation.PhysicalMemory = mainPhysicalMemory;
            simulation.FrameDivideStrategy = framesStrategy;
            
            simulation.ProcessOne();
            
            Assert.Equal(pages[0], frame1.Page);
            Assert.Null(frame2.Page);
            
            simulation.ProcessOne();
            
            Assert.Equal(pages[0], frame1.Page);
            Assert.Equal(pages[1], frame2.Page);
        }
    }
}