// populate the database and provide some sample data
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace OnlineStore.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices
                .GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            // if there are no objects in the database:
            // 1. populate using collection of Product objects (AddRange)
            // 2. write to the db (SaveChanges)
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                new Product
                {
                    Name = "High-back armchair",
                    Description = "The timeless design makes it easy to place in various room settings and match with other furniture.",
                    Category = "Sofas and armchairs",
                    Price = 225,
                    ImageUrl = "/images/high-back-armchair.jpg",
                    Quantity = 50
                },
                new Product
                {
                    Name = "Two-seat sofa",
                    Description = "Cover is made of velvet which, through a traditional weaving technique, gives the fabric a warm, deep colour and a soft surface with a dense pile and light, reflective shine.",
                    Category = "Sofas and armchairs",
                    Price = 895.90m,
                    ImageUrl = "/images/2-seat-sofa.jpg",
                    Quantity = 30
                },
                new Product
                {
                    Name = "Table",
                    Description = "The melamine table top is moisture resistant, stain resistant and easy to keep clean.",
                    Category = "Tables",
                    Price = 29.50m,
                    ImageUrl = "/images/table.jpg",
                    Quantity = 0
                },
                new Product
                {
                    Name = "Extendable table",
                    Description = "The smart design means that the table top has no seams when you use the table without extending it.",
                    Category = "Tables",
                    Price = 34.95m,
                    ImageUrl = "/images/extendable-table.jpg",
                    Quantity = 5
                },
                new Product
                {
                    Name = "Bar table",
                    Description = "Durable and hard-wearing; meets the requirements on furniture for public use.",
                    Category = "Tables",
                    Price = 65,
                    ImageUrl = "/images/bar-table.jpg",
                    Quantity = 10
                },
                new Product
                {
                    Name = "Chair",
                    Description = "You sit comfortably thanks to the high back and seat with polyester wadding.",
                    Category = "Chairs, stools and benches",
                    Price = 70,
                    ImageUrl = "/images/chair.jpg",
                    Quantity = 40
                },
                new Product
                {
                    Name = "Bar stool with backrest",
                    Description = "With footrest for relaxed sitting posture.",
                    Category = "Chairs, stools and benches",
                    Price = 45.50m,
                    ImageUrl = "/images/bar-stool-with-backrest.jpg",
                    Quantity = 400
                },
                new Product
                {
                    Name = "Stool",
                    Description = "Solid beech is a hardwearing natural material.",
                    Category = "Chairs, stools and benches",
                    Price = 20,
                    ImageUrl = "/images/stool.jpg",
                    Quantity = 35
                },
                new Product
                {
                    Name = "Chair with armrests",
                    Description = "The chair is easy to store when not in use, since you can stack up to 6 chairs on top of each other.",
                    Category = "Chairs, stools and benches",
                    Price = 55,
                    ImageUrl = "/images/chair-with-armrests.jpg",
                    Quantity = 25
                }
                );

                context.SaveChanges();
            }
        }
    }
}