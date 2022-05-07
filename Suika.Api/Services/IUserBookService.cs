using Suika.Api.Models;

namespace Suika.Api.Services;

public interface IUserBookService
{
    public Task<bool> CreateAsync(UserBook userBook);
    public Task<IEnumerable<object>> GetAllAsync(string username);
}