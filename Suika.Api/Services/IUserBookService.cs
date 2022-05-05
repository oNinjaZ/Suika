using Suika.Api.Models;

namespace Suika.Api.Services;

public interface IUserBookService
{
    public Task<bool> Create(UserBook userBook);
    public Task<IEnumerable<UserBook>> GetAll();
}