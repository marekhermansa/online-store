// this will stand in until real data storage
// has created

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public class TemporaryProductRepository : IProductRepository 
    {
        public IQueryable<Product> Products
        {
            get
            {
                List<Product> products = new List<Product> {
                    new Product { Name = "Bookcase", Price = 110 },
                    new Product { Name = "Coffe table", Price = 75 },
                    new Product { Name = "Cushion", Price = 20 },
                    new Product { Name = "Curtains", Price = 90 },
                    new Product { Name = "Kitchen chair", Price = 35 },
                    new Product { Name = "Desk lamp", Price = 25 },
                    new Product { Name = "Desk", Price = 155 }
                };

                return products.AsQueryable<Product>();
            }
        }

        //does nothing
        public void SaveProduct(Product product) { }
        public Product DeleteProduct(int productID)
        {
            return Products
                .FirstOrDefault(p => p.ProductID == productID);
        }
        //end
    }
}
