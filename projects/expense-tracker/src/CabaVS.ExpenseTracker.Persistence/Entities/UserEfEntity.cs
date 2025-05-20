using CabaVS.ExpenseTracker.Domain.Entities;

namespace CabaVS.ExpenseTracker.Persistence.Entities;

internal sealed class UserEfEntity
{
    public Guid Id { get; set; }

    public static UserEfEntity ConvertFromDomain(User user) => new() { Id = user.Id };

    public User ConvertToDomain() => new(Id);
}
