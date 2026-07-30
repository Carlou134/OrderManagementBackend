namespace OrderManagementBackend.Domain.Common;

public interface ICurrentUserProvider
{
    string GetCurrentUser();
}
