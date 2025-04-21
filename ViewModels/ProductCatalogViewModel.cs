using ProkodersECommerce.Models;

public class ProductCatalogViewModel
{
    public IEnumerable<Product> Products { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string SearchTerm { get; set; }
}
