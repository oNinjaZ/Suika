using Suika.Api.Models;

namespace Suika.Api.Services;

public interface IBookService
{
    public Task<bool> CreateAsync(Book book);
    public Task<IEnumerable<Book>> GetAllAsync();
}