using System.Collections.Generic;

namespace Shop_v2.Models
{
    public class ShoppingCartModel
    {
        public int Id { get; set; }

        public List<Cart> CartItems { get; set; }
         
        public decimal CartTotal { get; set; }
    }
}