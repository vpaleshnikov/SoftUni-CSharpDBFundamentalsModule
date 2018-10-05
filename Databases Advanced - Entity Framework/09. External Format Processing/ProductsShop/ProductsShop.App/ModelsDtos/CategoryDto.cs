namespace ProductsShop.App.ModelsDtos
{
    using System.Collections.Generic;
    using ProductsShop.Models;

    public class CategoryDto
    {
        public string Category { get; set; }

        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}