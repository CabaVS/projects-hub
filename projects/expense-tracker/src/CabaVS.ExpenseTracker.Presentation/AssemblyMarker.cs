using System.Reflection;

namespace CabaVS.ExpenseTracker.Presentation;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
