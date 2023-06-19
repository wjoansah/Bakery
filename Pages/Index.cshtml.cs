using Bakery.Data;
using Bakery.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Bakery.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly BakeryContext _bakeryContext;

    public List<Product> Products { get; set; } = new List<Product>();
    public Product FeaturedProduct { get; set; }

    public IndexModel(ILogger<IndexModel> logger, BakeryContext bakeryContext)
    {
        _logger = logger;
        _bakeryContext = bakeryContext;
    }

    public async Task OnGetAsync()
    {
        Products = await _bakeryContext.Products.ToListAsync();
        FeaturedProduct = Products.ElementAt(Random.Shared.Next(Products.Count));
    }

    //public void OnGet()
    //{
    //    Products = _bakeryContext.Products.ToList();
    //    FeaturedProduct = Products.ElementAt(Random.Shared.Next(Products.Count));
    //}
}
