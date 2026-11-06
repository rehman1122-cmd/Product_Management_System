using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Pages.Products;

public class EditModel(IProductRepository productRepository) : PageModel
{
    [BindProperty]
    public Product Product { get; set; } = new();

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
        if (id != Product.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        Product.Name = Product.Name.Trim();
        Product.Category = Product.Category.Trim();
        Product.Description = Product.Description?.Trim() ?? string.Empty;

        return await productRepository.UpdateAsync(Product)
            ? RedirectToPage("./Details", new { id })
            : NotFound();
    }
}
