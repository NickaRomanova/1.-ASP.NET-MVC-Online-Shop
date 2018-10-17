using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shop_v2.Models
{
    public class Producer
    { 
        public int Id { get; set; } 

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Введите наименование")]
        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Следует указать имя от 3 до 100 символов")]
        public string Name { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Введите описание")]
        [StringLength(10000, MinimumLength = 3, ErrorMessage = "Следует указать описание от 3 до 100 символов")]
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
        public Producer()
        {
            Products = new List<Product>();
        } 
    }
}