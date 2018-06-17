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

        public ViewResult List(string category, int productPage = 1, string filter = "default")
        {
            if (filter == "default")
                return View(new ProductsListViewModel
                {
                    Products = repository.Products
                        .Where(p => category == null || p.Category == category)
                        .OrderBy(p => p.ProductID) //refactor
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
            else if (filter == "priceLowest")
                return View(new ProductsListViewModel
                {
                    Products = repository.Products
                        .Where(p => category == null || p.Category == category)
                        .OrderBy(p => p.Price) //refactor
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
            else //if (filter == "priceHighest")
                return View(new ProductsListViewModel
                {
                    Products = repository.Products
                        .Where(p => category == null || p.Category == category)
                        .OrderByDescending(p => p.Price) //refactor
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
        }

        //public ViewResult List(string category, int productPage = 1, string filter = "default") =>
        //    View(new ProductsListViewModel
        //    {
        //        Products = repository.Products
        //            .Where(p => category == null || p.Category == category)
        //            .OrderBy(p => p.ProductID)
        //            .Skip((productPage - 1) * PageSize)
        //            .Take(PageSize),
        //        PagingInfo = new PagingInfo
        //        {
        //            CurrentPage = productPage,
        //            ItemsPerPage = PageSize,
        //            TotalItems = category == null ?
        //                repository.Products.Count() :
        //                repository.Products.Where(e =>
        //                e.Category == category).Count()
        //        },
        //        CurrentCategory = category
        //    });

        public ViewResult ProductPage(int productId, string returnUrl)
        {
            Product product = repository.Products
                .FirstOrDefault(p => p.ProductID == productId);

            ViewBag.ReturnUrl = returnUrl;

            return View(product);

            //return View(new ProductIndexViewModel
            //{
            //    Product = product,
            //    ReturnUrl = returnUrl
            //});
        }

        //public ViewResult Details(string productName)
        //{
        //    Product product = repository.Products
        //    .FirstOrDefault(p => p.Name == productName);

        //    return View("Details",product);
        //}
    }
}