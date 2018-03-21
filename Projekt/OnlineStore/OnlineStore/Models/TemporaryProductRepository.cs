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
        public IQueryable<Product> Products => new List<Product> {
            new Product { Name = "Bookcase", Price = 110 },
            new Product { Name = "Coffe table", Price = 75 },
            new Product { Name = "Cushion", Price = 20 },
            new Product { Name = "Curtains", Price = 90 },
            new Product { Name = "Floor lamp", Price = 55 }
        }.AsQueryable<Product>(); 
    }
}
