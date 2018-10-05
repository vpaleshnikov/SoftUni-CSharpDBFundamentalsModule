namespace ProductsShop.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.SoldProducts = new List<Product>();
            this.BoughtProducts = new List<Product>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public ICollection<Product> SoldProducts { get; set; }

        public ICollection<Product> BoughtProducts { get; set; }
    }
}
