using Suika.Api.Models;

namespace Suika.Api.Services;

public interface IUserService
{
    public Task<bool> Create(User user);
    public Task<bool> Update(User user);
    public Task<bool> Delete(string username);
    public Task<IEnumerable<User>> GetAll();
    public Task<User?> GetByUsermame(string username);
}