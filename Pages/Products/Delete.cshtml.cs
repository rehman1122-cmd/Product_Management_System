using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Pages.Products;

public class DeleteModel(IProductRepository productRepository) : PageModel
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        return await productRepository.DeleteAsync(id)
            ? RedirectToPage("/Index")
            : NotFound();
    }
}
