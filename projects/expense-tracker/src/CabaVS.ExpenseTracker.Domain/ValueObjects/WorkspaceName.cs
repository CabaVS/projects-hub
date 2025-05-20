using CabaVS.ExpenseTracker.Domain.Common;
using CabaVS.ExpenseTracker.Domain.Errors;
using CabaVS.ExpenseTracker.Domain.Primitives;

namespace CabaVS.ExpenseTracker.Domain.ValueObjects;

public sealed class WorkspaceName : ValueObject
{
    public const int MaxLength = 50;
    
    public string Value { get; }

    private WorkspaceName(string value) => Value = value;

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<WorkspaceName> Create(string workspaceName) =>
        Result<string>.Success(workspaceName)
            .EnsureStringNotNullOrWhitespace(WorkspaceErrors.NameIsNullOrWhitespace())
            .EnsureStringNotTooLong(MaxLength, WorkspaceErrors.NameIsTooLong(workspaceName))
            .Map(x => new WorkspaceName(x));
}
