using stepik.Db.Models;
using stepik_asp.Areas.AdminPanel.Models;
using stepik_asp.Models;
using System.Runtime.CompilerServices;

namespace stepik_asp.Helpers
{
    public static class Mapping
    {
        #region Product
        public static List<ProductViewModel> ToProductViewModels(this List<Product> productsDb)
        {
            var productsViewModel = new List<ProductViewModel>();

            foreach (var productDb in productsDb)
            {
                productsViewModel.Add(productDb.ToProductViewModel());
            }

            return productsViewModel;
        }

        public static ProductViewModel ToProductViewModel(this Product productDb)
        {
            return new ProductViewModel()
            {
                Id = productDb.Id,
                Name = productDb.Name,
                Cost = productDb.Cost,
                Description = productDb.Description,
                ImagePath = productDb.ImagePath
            };
        }

        public static Product ToProductDb(this ProductViewModel product)
        {
            return new Product()
            {
                Id = product.Id,
                Name = product.Name,
                Cost = product.Cost,
                Description = product.Description,
                ImagePath = product.ImagePath
            };
        }
        #endregion

        #region Cart
        public static List<CartItemViewModel> ToCartItemViewModels(this List<CartItem> cartDbItems)
        {
            var cartItemsViewModel = new List<CartItemViewModel>();

            foreach (var cartDbItem in cartDbItems)
            {
                cartItemsViewModel.Add(cartDbItem.ToCartItemViewModel());
            }

            return cartItemsViewModel;
        }

        public static CartItemViewModel ToCartItemViewModel(this CartItem cartDbItem)
        {
            return new CartItemViewModel()
            {
                Id = cartDbItem.Id,
                Product = cartDbItem.Product.ToProductViewModel(),
                Quantity = cartDbItem.Quantity,
                Cost = cartDbItem.Cost
            };
        }

        public static CartViewModel? ToCartViewModel(this Cart? cartDb)
        {
            if (cartDb == null)
            {
                return null;
            }

            return new CartViewModel()
            {
                Id = cartDb.Id,
                UserId = cartDb.UserId,
                Items = cartDb.Items.ToCartItemViewModels(),
            };
        }
        #endregion

        #region Favorite
        public static FavoriteViewModel? ToFavoriteViewModel(this Favorite favorite)
        {
            if (favorite == null)
            {
                return null;
            }

            return new FavoriteViewModel()
            {
                Id = favorite.Id,
                Items = favorite.Items.ToProductViewModels(),
                UserId = favorite.UserId,   
            };
        }
        #endregion

        #region Comparison
        public static ComparisonViewModel? ToComparisonViewModel(this Comparison comparison)
        {
            if (comparison == null)
            {
                return null;
            }

            return new ComparisonViewModel()
            {
                Id = comparison.Id,
                Items = comparison.Items.ToProductViewModels(),
                UserId = comparison.UserId
            };
        }
        #endregion

        #region Order

        public static OrderViewModel? ToOrderViewModel(this Order order)
        {
            return new OrderViewModel()
            {
                Id = order.Id,
                Items = order.Items.ToCartItemViewModels(),
                UserId = order.UserId,
                CreationDateTime = order.CreationDateTime,
                DeliveryUser = order.DeliveryUser.ToDeliveryUserViewModel(),
                Status = (Models.OrderStatus)order.Status,
                TotalCost = order.TotalCost,
                ItemsQuantity = order.ItemsQuantity
            };
        }
        public static List<OrderViewModel> ToOrderViewModels(this List<Order> ordersDb)
        {
            var ordersViewModel = new List<OrderViewModel>();

            foreach (var orderDb in ordersDb)
            {
                ordersViewModel.Add(orderDb.ToOrderViewModel());
            }

            return ordersViewModel;
        }


        public static DeliveryUserViewModel? ToDeliveryUserViewModel(this DeliveryUser deliveryUser)
        {
            return new DeliveryUserViewModel()
            {
                Id = deliveryUser.Id,
                Address = deliveryUser.Address,
                Comment = deliveryUser.Comment,
                Date = deliveryUser.Date,
                Name = deliveryUser.Name,
                Phone = deliveryUser.Phone
            };
        }

        public static DeliveryUser ToDeliveryUserDb(this DeliveryUserViewModel deliveryUser)
        {
            return new DeliveryUser()
            {
                Id = deliveryUser.Id,
                Name = deliveryUser.Name,
                Address = deliveryUser.Address,
                Phone = deliveryUser.Phone,
                Date = deliveryUser.Date,
                Comment = deliveryUser.Comment
            };
        }

        #endregion

        #region User
        public static UserViewModel ToUserViewModel(this User user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CreationDateTime = DateTime.UtcNow
            };
        }

        public static List<UserViewModel> ToUserViewModels(this List<User> users)
        {
            return users.Select(u => u.ToUserViewModel()).ToList();
        }


        #endregion
    }

}
