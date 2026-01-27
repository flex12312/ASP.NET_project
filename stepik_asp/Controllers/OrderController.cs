using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using stepik.Db.Interfaces;
using stepik.Db.Models;
using stepik_asp.Helpers;
using stepik_asp.Interfaces;
using stepik_asp.Models;

namespace stepik_asp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        public ICartsRepository _cartsRepository;
        public IOrdersRepository _ordersRepository;
        public OrderController(ICartsRepository IcartsRepository, IOrdersRepository IordersRepository)
        {
            _cartsRepository = IcartsRepository;
            _ordersRepository = IordersRepository;
        }
        public IActionResult Index()
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId, null);

            var order = new OrderViewModel()
            {
                Items = cart?.ToCartViewModel()?.Items
            };

            return View(order);
        }

        [HttpPost]
        public IActionResult Buy(OrderViewModel order)
        {
            var cart = _cartsRepository.TryGetByUserId(Constants.UserId, null);

            if (cart == null)
            {
                return View(nameof(Index), order);
            }

            order.Items = cart.ToCartViewModel()?.Items;
            order.UserId = Constants.UserId;

            if (!ModelState.IsValid)
            {
                return View(nameof(Index), order);
            }

            var orderItems = new List<CartItem>();
            foreach (var cartItem in cart.Items)
            {
                orderItems.Add(new CartItem
                {
                    Id = Guid.NewGuid(),
                    Product = cartItem.Product, 
                    Quantity = cartItem.Quantity
                });
            }

            var orderDb = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = order.UserId,
                Items = orderItems, 
                DeliveryUser = order.DeliveryUser.ToDeliveryUserDb(),
                CreationDateTime = DateTime.UtcNow,
                Status = stepik.Db.Models.OrderStatus.Created,
            };

            _ordersRepository.Add(orderDb);
            _cartsRepository.Clear(Constants.UserId, null);

            return RedirectToAction(nameof(Success));
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}