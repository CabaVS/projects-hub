namespace CabaVS.ExpenseTracker.Domain.Common;

public sealed record ValidationSummaryError(IEnumerable<Error> NestedErrors) 
    : Error("Validation.Summary", $"One or more validation failures occured. See '{nameof(NestedErrors)}' for more details.");
