using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stepik.Db.Models
{
    public class Cart
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }  
        public string? GuestId { get; set; }
        public List<CartItem> Items { get; set; }

    }
}