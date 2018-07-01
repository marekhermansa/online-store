//534
using Microsoft.AspNetCore.Identity;

namespace OnlineStore.Models
{
    public class AppUser : IdentityUser
    {
        //address
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        //credit card
        public string CreditCardOwner { get; set; }
        public string CreditCardNumber { get; set; }
        public string ExpirationDate { get; set; }
    }
}