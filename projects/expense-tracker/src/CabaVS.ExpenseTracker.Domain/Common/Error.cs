﻿namespace CabaVS.ExpenseTracker.Domain.Common;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);
}
