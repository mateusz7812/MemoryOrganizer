using System;
using System.Collections.Generic;
using System.Linq;
using MemoryOrganizer.Frames;
using MemoryOrganizer.Memories;
using MemoryOrganizer.OrganizingStrategies;
using MemoryOrganizer.PageReferences;
using Moq;
using Xunit;

namespace XUnitTestProject1
{
    public class TestRandom
    {
        [Fact]
        public void Test1()
        {
            var frames = new List<IFrame>()
            {
                Mock.Of<IFrame>(m => m.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(m => m.Id == Guid.NewGuid()),
                Mock.Of<IFrame>(m => m.Id == Guid.NewGuid()),
            };
            var physicalMemory = Mock.Of<IPhysicalMemory>();
            Mock.Get(physicalMemory).Setup(m => m.GetEnumerator()).Returns(frames.GetEnumerator());
            var strategy = new RandomStrategy(physicalMemory);
            for (var i = 0; i < 10; i++)
            {
                var pageId = Guid.NewGuid();

                var visitor = strategy.CreateReplacingVisitor(Mock.Of<IPageReference>(m => m.PageId == pageId));
                Assert.Contains(visitor.FrameId, frames.Select(f => f.Id));
                Assert.Equal(pageId, visitor.PageId);
            }
            Assert.Equal(10, strategy.PagesErrors);
        }
    }
}