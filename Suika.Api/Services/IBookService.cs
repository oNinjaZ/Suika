using Suika.Api.Models;

namespace Suika.Api.Services;

public interface IBookService
{
    public Task<bool> Create(Book book);
}