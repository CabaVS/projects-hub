using System.Reflection;

namespace CabaVS.ExpenseTracker.Persistence;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
