using Microsoft.EntityFrameworkCore;
using stepik.Db;
using stepik.Db.Models;
using stepik_asp.Interfaces;
using stepik_asp.Models;

namespace stepik_asp.Repositories
{
    public class OrdersDbRepository : IOrdersRepository
    {
        private readonly DatabaseContext _databaseContext;
        public OrdersDbRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }


        public void Add(Order order)
        {
            order.Id = Guid.NewGuid();
            order.CreationDateTime = DateTime.UtcNow;
            if (order.DeliveryUser != null)
            {
                order.DeliveryUser.Id = Guid.NewGuid();
                order.DeliveryUser.Date = DateTime.UtcNow;
            }

            order.Status = OrderStatus.Created;
            if (order.Items != null)
            {
                foreach (var item in order.Items)
                {
                    item.Id = Guid.NewGuid(); 
                    item.Order = order; 
                }
            }
            _databaseContext.Orders.Add(order);
            _databaseContext.SaveChanges();
        }



        public List<Order> GetAll() => _databaseContext.Orders.
            Include(x => x.DeliveryUser).
            Include(x => x.Items).
            ThenInclude(x => x.Product).ToList();

        public Order? TryGetById(Guid orderId) =>
            _databaseContext.Orders.Include(x => x.DeliveryUser)
            .Include(x => x.Items).
            ThenInclude(x => x.Product).
            FirstOrDefault(order => order.Id == orderId);



        public void UpdateStatus(Guid orderId, OrderStatus newStatus)
        {
            var order = TryGetById(orderId);
            if (order != null)
            {
                order.Status = newStatus;
            }
            _databaseContext.SaveChanges();
        }
    }
}