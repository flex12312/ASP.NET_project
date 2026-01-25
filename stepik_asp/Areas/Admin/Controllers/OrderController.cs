using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using stepik.Db;
using stepik_asp.Areas.AdminPanel.Models;
using stepik_asp.Helpers;
using stepik_asp.Interfaces;
using stepik_asp.Models;

namespace stepik_asp.Areas.AdminPanel.Controllers
{
    [Area(Consts.AdminRoleName)]
    [Authorize(Roles = Consts.AdminRoleName)]
    public class OrderController(IOrdersRepository _ordersRepository) : Controller
    {
    #region Orders
        public IActionResult Orders()
        {
            var orders = _ordersRepository.GetAll();
            return View(orders?.ToOrderViewModels());
        }

        public IActionResult DetailOrder(Guid orderId)
        {
            var order = _ordersRepository.TryGetById(orderId);
            return View(order?.ToOrderViewModel());
        }

        public IActionResult UpdateOrder(Guid orderId, OrderStatus status)
        {
            _ordersRepository.UpdateStatus(orderId, (stepik.Db.Models.OrderStatus)status);
            return RedirectToAction("Orders");
        }
        #endregion
    }
}