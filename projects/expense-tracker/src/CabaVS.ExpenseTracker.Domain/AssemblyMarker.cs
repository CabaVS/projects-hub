using System.Reflection;

namespace CabaVS.ExpenseTracker.Domain;

public static class AssemblyMarker
{
    public static readonly Assembly Assembly = typeof(AssemblyMarker).Assembly;
}
