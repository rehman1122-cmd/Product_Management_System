using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Pages;

public class IndexModel(IProductRepository productRepository) : PageModel
{
    public IReadOnlyList<Product> Products { get; private set; } = [];
    public int CategoryCount => Products.Select(product => product.Category).Distinct(StringComparer.OrdinalIgnoreCase).Count();
    public int TotalStock => Products.Sum(product => product.Stock);

    public async Task OnGetAsync()
    {
        Products = await productRepository.GetAllAsync();
    }
}
