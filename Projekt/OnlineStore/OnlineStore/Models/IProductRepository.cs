// logic for storing and retrieving the data from 
// the persistent data store

using System.Linq;

namespace OnlineStore.Models
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productID);
    }
}
