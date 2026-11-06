using Microsoft.Data.Sqlite;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Data;

public sealed class ProductRepository(SqliteConnectionFactory connectionFactory) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        var products = new List<Product>();
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, Name, PriceCents, Category, Stock, Description
            FROM Products
            ORDER BY Name COLLATE NOCASE;
            """;

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            products.Add(ReadProduct(reader));
        }

        return products;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            SELECT Id, Name, PriceCents, Category, Stock, Description
            FROM Products
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", id);

        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync() ? ReadProduct(reader) : null;
    }

    public async Task<int> AddAsync(Product product)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            INSERT INTO Products (Name, PriceCents, Category, Stock, Description)
            VALUES ($name, $priceCents, $category, $stock, $description);
            SELECT last_insert_rowid();
            """;
        AddProductParameters(command, product);

        return Convert.ToInt32(await command.ExecuteScalarAsync());
    }

    public async Task<bool> UpdateAsync(Product product)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = """
            UPDATE Products
            SET Name = $name,
                PriceCents = $priceCents,
                Category = $category,
                Stock = $stock,
                Description = $description
            WHERE Id = $id;
            """;
        AddProductParameters(command, product);
        command.Parameters.AddWithValue("$id", product.Id);

        return await command.ExecuteNonQueryAsync() == 1;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        await using var connection = connectionFactory.CreateConnection();
        await connection.OpenAsync();

        await using var command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Products WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", id);

        return await command.ExecuteNonQueryAsync() == 1;
    }

    private static void AddProductParameters(SqliteCommand command, Product product)
    {
        command.Parameters.AddWithValue("$name", product.Name);
        command.Parameters.Add("$priceCents", SqliteType.Integer).Value = decimal.ToInt64(product.Price * 100m);
        command.Parameters.AddWithValue("$category", product.Category);
        command.Parameters.AddWithValue("$stock", product.Stock);
        command.Parameters.AddWithValue("$description", product.Description ?? string.Empty);
    }

    private static Product ReadProduct(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt32(0),
        Name = reader.GetString(1),
        Price = reader.GetInt64(2) / 100m,
        Category = reader.GetString(3),
        Stock = reader.GetInt32(4),
        Description = reader.GetString(5)
    };
}
