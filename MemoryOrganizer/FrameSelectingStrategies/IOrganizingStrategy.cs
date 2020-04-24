using MemoryOrganizer.Memories;
using MemoryOrganizer.PageReferences;
using MemoryOrganizer.ReplacingVisitors;

namespace MemoryOrganizer.OrganizingStrategies
{
    public interface IOrganizingStrategy
    {
        string Name { get; }
        int PagesErrors { get; }
        IReplacingVisitor CreateReplacingVisitor(IPageReference pageReference);
    }
}
