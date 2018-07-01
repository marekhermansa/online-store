using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OnlineStore.Infrastructure;
using System;

namespace OnlineStore.Models
{
    public class SessionCart : Cart
    {
        // factory for creating SessionCart objects
        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?
            .HttpContext.Session;
            SessionCart cart = session?.GetJson<SessionCart>("Cart")
            ?? new SessionCart();
            cart.Session = session;
            return cart;
        }
        [JsonIgnore]
        public ISession Session { get; set; }
        public override void AddItem(Product product, int quantity)
        {
            // call the base implementations
            base.AddItem(product, quantity);
            // store the updated state in the session
            Session.SetJson("Cart", this);
        }
        public override void RemoveLine(Product product)
        {
            base.RemoveLine(product);
            Session.SetJson("Cart", this);
        }
        public override void Clear()
        {
            base.Clear();
            Session.Remove("Cart");
        }
    }
}