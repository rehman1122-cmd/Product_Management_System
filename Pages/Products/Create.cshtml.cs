using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProductManager.Models;
using ProductManager.Repositories;

namespace ProductManager.Pages.Products;

public class CreateModel(IProductRepository productRepository) : PageModel
{
    [BindProperty]
    public Product Product { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        NormalizeProduct();
        await productRepository.AddAsync(Product);
        return RedirectToPage("/Index");
    }

    private void NormalizeProduct()
    {
        Product.Name = Product.Name.Trim();
        Product.Category = Product.Category.Trim();
        Product.Description = Product.Description?.Trim() ?? string.Empty;
    }
}
