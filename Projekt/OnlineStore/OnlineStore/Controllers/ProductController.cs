using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;
using OnlineStore.Models.ViewModels;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        // dependecy injection
        private IProductRepository repository;
        public int PageSize = 6;

        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        //// render the default view for the action method
        // pass a ProductsListViewModel object as the model data to the view
        public ViewResult List(string category, int productPage = 1)
            => View(new ProductsListViewModel{
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo{
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        repository.Products.Count() :
                        repository.Products.Where(e => 
                        e.Category == category).Count()
                },
                CurrentCategory = category
            });
    }
}