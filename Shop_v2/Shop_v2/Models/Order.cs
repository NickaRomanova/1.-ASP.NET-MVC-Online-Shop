using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shop_v2.Models
{
    // Этот класс отслеживает сведения сводки и доставки для заказа
    public class Order
    {
        public int OrderId { get; set; }
        //public string Username { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        //public string Address { get; set; }
        //public string City { get; set; }
        //public string State { get; set; }
        //public string PostalCode { get; set; }
        //public string Country { get; set; }
        //public string Phone { get; set; }
        //public string Email { get; set; }
        public  int UserId { get; set; }
        public decimal Total { get; set; }
        public System.DateTime OrderDate { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}