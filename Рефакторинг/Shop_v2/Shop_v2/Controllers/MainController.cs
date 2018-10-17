using Shop_v2.Context; 
using System.Linq; 
using System.Web.Mvc;
using System.Data.Entity;
using Shop_v2.Models;

namespace Shop_v2.Controllers
{
    public class MainController : Controller
    {
        Shop_version_2_Context db = new Shop_version_2_Context();
         
        public ActionResult Index()
        {
            var products = db.Products.Include(t => t.Category)
                                         .OrderBy(t => t.Id);
            return View(products.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Product product = db.Products.Include(t => t.Producer)
                                         .Include(t => t.Category)
                                         .FirstOrDefault(t => t.Id == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            return View(product);
        }
    }
}