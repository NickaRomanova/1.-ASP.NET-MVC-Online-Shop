using System;
using System.Collections.Generic; 

namespace Shop_v2.Models
{
    public class HistoryOrder
    {
        public int Id { get; set; }

        public int UserId { get; set; } 

        public ICollection<Product> Products { get; set; }
        public HistoryOrder()
        {
            Products = new List<Product>();
        }

        public DateTime DateBuy { get; set; }
    }
}