using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Models.ViewModels
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }
        // display the following url if the user clicks 
        // the ContinueShopping button
        public string ReturnUrl { get; set; }
    }
}