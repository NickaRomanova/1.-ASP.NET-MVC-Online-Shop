using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using System.Web;

namespace Shop_v2.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Наименование")]
        [Required(ErrorMessage = "Введите наименование")]
        [StringLength(10000, MinimumLength = 3, ErrorMessage = "Следует указать имя от 3 до 100 символов")]
        public string Name { get; set; }

        [Display(Name = "Изображение")]
        public string ImagePath { get; set; }
        public string Title { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        public Producer Producer { get; set; }

        [Display(Name = "Производитель")]
        [Required(ErrorMessage = "Выберите производителя из списка или добавте в настрйках нового.")]
        public int? ProducerId { get; set; }

        public Category Category { get; set; }  

        [Display(Name = "Категория")]
        [Required(ErrorMessage = "Выберите категорию из списка или добавте в настрйках новую.")] 
        public int? CategoryId { get; set; }  

        [Display(Name = "Характеристики")]
        [Required(ErrorMessage = "Введите характеристики")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000000, MinimumLength = 5, ErrorMessage = "Следует указать характеристики от 5 до 1000000 символов")]
        public string Characteristic { get; set; }

        [Display(Name = "Описание")]
        [Required(ErrorMessage = "Введите описание")]
        [DataType(DataType.MultilineText)]
        [StringLength(1000000, MinimumLength = 5, ErrorMessage = "Следует указать описание от 5 до 1000000 символов")]
        public string Description { get; set; }

        [Display(Name = "Количество")]
        [Required(ErrorMessage = "Введите количество товара на складе")]
        public int Quantity { get; set; }

        [Display(Name = "Цена")]
        [Required(ErrorMessage = "Введите цену товара")]
        public int Price { get; set; } 
    }
}