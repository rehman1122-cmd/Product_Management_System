using Microsoft.Data.Sqlite;

namespace ProductManager.Data;

public sealed class SqliteConnectionFactory(string connectionString)
{
    public SqliteConnection CreateConnection() => new(connectionString);
}
