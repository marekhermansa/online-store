// logic for storing and retrieving the data from 
// the persistent data store

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public interface IProductRepository
    {
        // allow a caller to obtain a sequence of Product objects
        IQueryable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productID);
    }
}
