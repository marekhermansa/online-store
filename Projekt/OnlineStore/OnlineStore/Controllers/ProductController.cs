using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class ProductController : Controller
    {
        // dependecy injection
        private IProductRepository repository;
        public ProductController(IProductRepository repo)
        {
            repository = repo;
        }

        // render the default view for the action method
        public ViewResult List()
        {
            return View(repository.Products);
        }
    }
}