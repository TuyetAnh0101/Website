using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SportsStore.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SportsStore.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            IStoreRepository repo = services.GetRequiredService<IStoreRepository>();

            List<CartLineSession>? cartSession = session?.GetJson<List<CartLineSession>>("Cart");
            SessionCart cart = new SessionCart();

            if (cartSession != null)
            {
                foreach (var item in cartSession)
                {
                    var product = repo.Products.FirstOrDefault(p => p.ProductID == item.ProductID);

                    if (product != null)
                    {
                        Console.WriteLine($"[SessionCart] Load line: {product.ProductID}, {product.Price}, {product.RentPrice}");

                        cart.Lines.Add(new CartLine
                        {
                            Product = product,
                            Quantity = item.Quantity,
                            IsRental = item.IsRental,
                            RentalDays = item.RentalDays
                        });
                    }
                    else
                    {
                        Console.WriteLine($"[SessionCart] ProductID {item.ProductID} not found in DB.");
                    }
                }
            }

            cart.Session = session;
            return cart;
        }

        public ISession? Session { get; set; }

        public override void AddItem(Product product, int quantity, bool isRental = false, int rentalDays = 0)
        {
            Console.WriteLine($"[SessionCart] AddItem: {product.ProductID}, {product.Price}, {product.RentPrice}");
            base.AddItem(product, quantity, isRental, rentalDays);
            SaveCartToSession();
        }

        public override void RemoveLine(Product product, bool isRental = false)
        {
            base.RemoveLine(product, isRental);
            SaveCartToSession();
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }

        private void SaveCartToSession()
        {
            var cartSession = Lines.Select(line => new CartLineSession
            {
                ProductID = line.Product.ProductID ?? 0,
                Quantity = line.Quantity,
                IsRental = line.IsRental,
                RentalDays = line.RentalDays
            }).ToList();

            foreach (var item in cartSession)
            {
                Console.WriteLine($"[SessionCart] Save line: {item.ProductID}, Qty: {item.Quantity}, Rental: {item.IsRental}, Days: {item.RentalDays}");
            }

            Session?.SetJson("Cart", cartSession);
        }
    }
}
