﻿using System.Diagnostics.CodeAnalysis;

namespace CabaVS.ExpenseTracker.Domain.Primitives;

[SuppressMessage(
    "Major Code Smell",
    "S4035:Classes implementing \"IEquatable<T>\" should be sealed", 
    Justification = "Target type is taken into account.")]
public abstract class Entity(Guid id) : IEquatable<Entity>
{
    public Guid Id { get; } = id;
    
    public static bool operator ==(Entity? first, Entity? second) =>
        first is not null &&
        first.Equals(second);
    
    public static bool operator !=(Entity? first, Entity? second) =>
        !(first == second);
    
    public bool Equals(Entity? other) =>
        other is not null && 
        other.GetType() == GetType() &&
        other.Id == Id;

    public override bool Equals(object? obj) =>
        obj is Entity auditableEntity &&
        Equals(auditableEntity);
    
    public override int GetHashCode() =>
        HashCode.Combine(
            Id.GetHashCode(),
            GetType().GetHashCode()) * 47;
}
