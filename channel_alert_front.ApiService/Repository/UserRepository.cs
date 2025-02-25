using Microsoft.EntityFrameworkCore.Query;

using channel_alert_front.ApiService.DB;
using channel_alert_front.ApiService.Entities;

namespace channel_alert_front.ApiService.Repository;

public interface IUserRepository : IRepositoryBase<User>
{
    public IEnumerable<User> GetAllUsers();
    public Task<bool> CreateAsync(User user);
    public Task<int> DeleteAsync(string email);
    public Task<int> UpdateToken(string email, string? token);
}

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    public UserRepository(RepositoryContext repositoryContext) : base(repositoryContext)
    {
    }

    public IEnumerable<User> GetAllUsers()
    {
        return FindAll().OrderBy(user => user.UserName);
    }

    public async Task<bool> CreateAsync(User user)
    {
        IQueryable<User> foundUser = FindByCondition((dbUser) => dbUser.Email == user.Email);
        if (foundUser.Any())
        {
            return false;
        }


        await RepositoryContext.User.AddAsync(user);
        bool saved = await SaveAsync();
        return saved;
    }

    public async Task<int> DeleteAsync(string email)
    {
        // Soft Delete (X) : Disable row & revoke tokens
        // Hard Delete (O) : Because it is a Personal Appplication
        return await DeleteAsync((user) => user.Email == email);
    }

    public async Task<int> UpdateToken(string email, string? token)
    {
        int updated = await UpdateAsync(
            (User user) => user.Email == email,
            (SetPropertyCalls<User> user) => user.SetProperty(a => a.RefreshToken, token)
        );

        return updated;
    }
}