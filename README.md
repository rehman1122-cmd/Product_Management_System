# Stockroom

A small ASP.NET Core Razor Pages app for managing a product catalog. It uses
SQLite through `Microsoft.Data.Sqlite` and a parameterized ADO.NET repository.

## Run locally

Requires the .NET 10 SDK.

```sh
dotnet run --project ProductManager
```

The app creates `ProductManager/App_Data/products.db` and its `Products` table
on first startup. Product data remains in that file between runs.

## Project structure

- `Models/Product.cs` — product fields and validation rules.
- `Repositories/IProductRepository.cs` — repository contract.
- `Data/ProductRepository.cs` — SQLite CRUD queries using ADO.NET.
- `Data/DatabaseInitializer.cs` — creates the database schema.
- `Pages/Products/` — create, details, edit, and delete pages.
- `Pages/Index.cshtml` — inventory overview.
