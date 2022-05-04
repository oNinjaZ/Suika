using Suika.Api.Models;

namespace Suika.Api.Services;

public interface IUserService
{
    public Task<bool> CreateAsync(User user);
    public Task<bool> UpdateAsync(User user);
    public Task<bool> DeleteAsync(string username);
    public Task<IEnumerable<User>> GetAllAsync();
    public Task<User?> GetByUsernameAsync(string username);
    public Task<IEnumerable<User>> SearchByUsernameAsync(string searchTerm);
}