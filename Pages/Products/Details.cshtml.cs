using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Pages.Products;

public class DetailsModel(IProductRepository productRepository) : PageModel
{
    public Product Product { get; private set; } = new();

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product is null)
        {
            return NotFound();
        }

        Product = product;
        return Page();
    }
}
