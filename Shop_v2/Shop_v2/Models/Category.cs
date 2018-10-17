using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; 

namespace Shop_v2.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Display(Name = "Название категории")]
        [Required(ErrorMessage = "Введите категорию")] 
        [StringLength(1000, MinimumLength = 3, ErrorMessage = "Следует указать название категории от 3 до 1000 символов")]
        public string CategoryName { get; set; }
         
        public ICollection<Product> Products { get; set; }
        public Category()
        {
            Products = new List<Product>();
        }
    }
}