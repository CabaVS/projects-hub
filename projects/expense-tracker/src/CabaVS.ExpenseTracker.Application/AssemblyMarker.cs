using System.Reflection;

namespace CabaVS.ExpenseTracker.Application;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
