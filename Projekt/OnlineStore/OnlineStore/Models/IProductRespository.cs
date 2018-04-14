using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    interface IProductRespository
    {
        IQueryable<Product> Products { get; }
        void SaveProduct(Product product);
    }
}
