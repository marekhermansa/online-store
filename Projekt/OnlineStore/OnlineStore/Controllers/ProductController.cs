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
        private IProductRepository repository;

        public ProductController(IProductRepository repository) =>
            this.repository = repository;

        //public ViewResult List() => View(repository.Products);

        public int PageSize = 4;

        public ViewResult List(string category, int productPage = 1) =>
            View(new ProductsListViewModel
            {
                Products = repository.Products
                    .Where(p => category == null || p.Category == category)
                    .OrderBy(p => p.ProductID)
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = productPage,
                    ItemsPerPage = PageSize,
                    TotalItems = category == null ?
                        repository.Products.Count() :
                        repository.Products.Where(e =>
                        e.Category == category).Count()
                },
                CurrentCategory = category
            });

        private Product product;

        //public ViewResult Index(string returnUrl)
        //{
        //    return View(new ProductIndexViewModel
        //    {
        //        Product = product,
        //        ReturnUrl = returnUrl
        //    });
        //}
        public ViewResult /*RedirectToActionResult*/ProductPage(int productId, string returnUrl)
        {
            Product product = repository.Products
            .FirstOrDefault(p => p.ProductID == productId);

            return View(new ProductIndexViewModel
            {
                Product = product,
                ReturnUrl = returnUrl
            });

            //return RedirectToAction("Index", new { returnUrl });
        }
    }
}