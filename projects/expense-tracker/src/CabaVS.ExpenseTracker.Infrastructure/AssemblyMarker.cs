using System.Reflection;

namespace CabaVS.ExpenseTracker.Infrastructure;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
