using Microsoft.EntityFrameworkCore;
using stepik.Db.Interfaces;
using stepik.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Db.Repositories
{
    public class CartsDbRepository : ICartsRepository
    {
        private readonly DatabaseContext _databaseContext;

        public CartsDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public Cart? TryGetByUserId(string userId)
        {
            return _databaseContext.Carts.Include(x => x.Items)
                .ThenInclude(x => x.Product).FirstOrDefault(x => x.UserId == userId);
        }

        public void Add(Product product, string userId)
        {
            var existingCart = TryGetByUserId(userId);

            if (existingCart == null)
            {
                existingCart = new Cart()
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };

                existingCart.Items = new List<CartItem>()
                    {
                        new CartItem()
                        {
                            Product = product,
                            Quantity = 1,
                            Cart = existingCart
                        }

                };
                _databaseContext.Carts.Add(existingCart);
            }
            else
            {
                var existingCartItem = existingCart.Items
                    .FirstOrDefault(item => item.Product.Id == product.Id);

                if (existingCartItem == null)
                {
                    var newCartItem = new CartItem()
                    {
                        Product = product,
                        Quantity = 1,
                        Cart = existingCart
                    };
                    existingCart.Items.Add(newCartItem);
                }
                else
                {
                    existingCartItem.Quantity++;
                }
            }

            _databaseContext.SaveChanges();  
        }

        public void Decrease(int productId, string userId)
        {
            var existingCart = TryGetByUserId(userId);

            var existingCartItem = existingCart?.Items
                .FirstOrDefault(item => item.Product.Id == productId);

            if (existingCartItem == null)
            {
                return;
            }

            existingCartItem.Quantity--;

            if (existingCartItem.Quantity == 0)
            {
                existingCart?.Items.Remove(existingCartItem);
            }

            _databaseContext.SaveChanges();  
        }

        public void Clear(string userId)
        {
            var existingCart = TryGetByUserId(userId);

            if (existingCart != null)
            {
                _databaseContext.Carts.Remove(existingCart);

                _databaseContext.SaveChanges(); 
            }
        }
    }
}
