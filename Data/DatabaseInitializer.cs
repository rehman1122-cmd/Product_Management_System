namespace ProductManager.Data;

public sealed class DatabaseInitializer(SqliteConnectionFactory connectionFactory)
{
    public async Task InitializeAsync()
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS Products (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                PriceCents INTEGER NOT NULL CHECK (PriceCents > 0),
                Category TEXT NOT NULL,
                Stock INTEGER NOT NULL CHECK (Stock >= 0),
                Description TEXT NOT NULL DEFAULT ''
            );
            """;
        await command.ExecuteNonQueryAsync();
    }
}
