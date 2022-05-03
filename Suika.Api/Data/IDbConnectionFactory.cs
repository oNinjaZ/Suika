using System.Data;

namespace Suika.Api.Data;

public interface IDbConnectionFactory
{
    public Task<IDbConnection> CreateConnectionAsync();
}