using System.ComponentModel.DataAnnotations;

namespace stepik.Db.Models
{
    public enum OrderStatus
    {
        Created,
        Processed,
        InTransit,
        Delivered,
        Cancelled
    }
}