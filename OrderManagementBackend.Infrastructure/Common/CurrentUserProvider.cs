using OrderManagementBackend.Domain.Common;

namespace OrderManagementBackend.Infrastructure.Common;

// TODO: replace with the authenticated user's identity once auth is implemented.
public class CurrentUserProvider : ICurrentUserProvider
{
    public string GetCurrentUser() => "root";
}
