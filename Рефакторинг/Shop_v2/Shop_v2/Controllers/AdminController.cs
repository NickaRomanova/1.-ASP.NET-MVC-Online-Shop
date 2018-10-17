using System;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using Shop_v2.Models;
using Shop_v2.Context;
using System.IO;

namespace Shop_v2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        Shop_version_2_Context db = new Shop_version_2_Context();

        public ActionResult Index()
        {
            if (HttpContext.User.Identity.Name.ToString() == "admin")
            {
                var products = db.Products.Include(p => p.Producer)
                                      .Include(p => p.Category);
                return View(products.ToList());
            }

            else
            {
                return RedirectToAction("Login", "MyAccount");
            }
        }

        [HttpGet]
        public ActionResult DetailsProduct(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Product product = db.Products.Include(t => t.Producer).FirstOrDefault(t => t.Id == id);

            Product product1 = db.Products.Include(m => m.Category).FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            if (product.Id != product1.Id)
            {
                return HttpNotFound();
            }
            else
                return View(product);
        }

        [HttpGet]
        public ActionResult EditProduct(int? id)
        {
            SelectList producer = new SelectList(db.Producers, "Id", "Name");
            ViewBag.Producers = producer;

            SelectList category = new SelectList(db.Categories, "Id", "CategoryName");
            ViewBag.Categories = category;

            if (id == null)
            {
                return HttpNotFound();
            }
            Product product = db.Products.Find(id);

            if (product != null)
            {
                return View(product);
            }

            return HttpNotFound();
        }

        [HttpPost]
        public ActionResult EditProduct(Product product)
        {
            int temp = product.Id;
            db.Entry(product).State = EntityState.Modified;

            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                string extension = Path.GetExtension(product.ImageFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                product.ImagePath = "~/Content/Images/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                product.ImageFile.SaveAs(fileName);

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateProduct(Product product)
        {
            SelectList producers = new SelectList(db.Producers, "Id", "Name", "Выберите поставщика");
            ViewBag.Producers = producers;

            SelectList categories = new SelectList(db.Categories, "Id", "CategoryName");
            ViewBag.Categories = categories;
            if (ModelState.IsValid)
            {
                var item = db.Products.FirstOrDefault(c => c.Name == product.Name);

                if (item != null)
                {
                    ViewBag.Message = "Такой товар уже существует. Введите уникальное название.";
                    return View();
                }
                else
                {
                    string fileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName);
                    string extension = Path.GetExtension(product.ImageFile.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    product.ImagePath = "~/Content/Images/" + fileName;
                    fileName = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    product.ImageFile.SaveAs(fileName);

                    db.Products.Add(product);
                    db.SaveChanges();
                    ModelState.Clear();
                    return RedirectToAction("Index", "Admin");
                }
            }
            else
            {
                return View(product);
            }
        }

        [HttpGet]
        public ActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Include(t => t.Producer).FirstOrDefault(t => t.Id == id);
            Product product1 = db.Products.Include(t => t.Category).FirstOrDefault(t => t.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            if (product.Id != product.Id)
            {
                return HttpNotFound();
            }
            else
                return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        public ActionResult DeleteConfirm(int id)
        {
            Product b = db.Products.Find(id);
            if (b == null)
            {
                return HttpNotFound();
            }
            db.Products.Remove(b);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult AllCategory()
        {
            var categories = db.Categories.OrderBy(c => c.CategoryName);
            return View(categories.ToList());
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult CreateCategory(Category category)
        {
            if (ModelState.IsValid)
            {

                var item = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);

                if (item != null)
                {
                    ViewBag.Message = "Такая категория уже существует. Введите уникальное название.";
                    return View();
                }
                else
                {
                    db.Categories.Add(category);
                    db.SaveChanges();
                    return RedirectToAction("AllCategory", "Admin");
                }
            }
            else
            {
                return View(category);
            }
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditCategory(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }

            Category category = db.Categories.Include(t => t.Products).FirstOrDefault(t => t.Id == Id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                var item = db.Categories.FirstOrDefault(c => c.CategoryName == category.CategoryName);

                if (item != null)
                {
                    ViewBag.Message = "Такая категория уже существует. Введите уникальное название.";
                    return View();
                }
                else
                {
                    db.Entry(category).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AllCategory", "Admin");
                }

            }
            else
            {
                return View(category);
            }
        }

        [HttpGet]
        public ActionResult DeleteCategory(int? Id)
        {
            Category category = db.Categories.Include(t => t.Products).FirstOrDefault(t => t.Id == Id);

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Find(id);

            var products = db.Products.Where(p => p.CategoryId == id);


            foreach (var item in products)
            {
                db.Products.Remove(item);
            }

            db.Categories.Remove(category);
            db.SaveChanges();
            return RedirectToAction("AllCategory", "Admin");
        }

        [HttpGet]
        public ActionResult AllProducer()
        {
            var producers = db.Producers;
            return View(producers.ToList());
        }

        [HttpGet]
        public ActionResult AllUsers()
        {
            var users = db.Users;
            return View(users.ToList());
        }

        [HttpGet]
        public ActionResult DetailsProducer(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }
            Producer producer = db.Producers.Find(Id);

            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        [HttpGet]
        public ActionResult EditProducer(int? Id)
        {
            if (Id == null)
            {
                return HttpNotFound();
            }

            Producer producer = db.Producers.Include(t => t.Products).FirstOrDefault(t => t.Id == Id);

            if (producer == null)
            {
                return HttpNotFound();
            }
            return View(producer);
        }

        [HttpPost]
        public ActionResult EditProducer(Producer producer)
        {
            if (ModelState.IsValid)
            {
                var item = db.Producers.FirstOrDefault(c => c.Name == producer.Name);

                if (item != null)
                {
                    ViewBag.Message = "Такой поставщик уже существует. Введите уникальное название.";
                    return View();
                }
                else
                {
                    db.Entry(producer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("AllProducer", "Admin");
                }
            }
            else
            {
                return View(producer);
            }

        }

        [HttpGet]
        public ActionResult DeleteProducer(int? Id)
        {

            Producer producer = db.Producers.Include(t => t.Products).FirstOrDefault(t => t.Id == Id);

            if (producer == null)
            {
                return HttpNotFound();
            }

            return View(producer);
        }

        [HttpPost]
        public ActionResult DeleteProducer(int id)
        {
            Producer producer = db.Producers.Find(id);

            var products = db.Products.Where(p => p.ProducerId == id);

            foreach (var item in products)
            {
                db.Products.Remove(item);
            }

            db.Producers.Remove(producer);
            db.SaveChanges();
            return RedirectToAction("AllProducer", "Admin");
        }

        [HttpGet]
        public ActionResult CreateProducer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateProducer(Producer producer)
        {
            if (ModelState.IsValid)
            {
                var item = db.Producers.FirstOrDefault(c => c.Name == producer.Name);

                if (item != null)
                {
                    ViewBag.Message = "Такой производитель уже существует. Введите уникальное название.";
                    return View();
                }
                else
                {
                    db.Producers.Add(producer);
                    db.SaveChanges();
                    return RedirectToAction("AllProducer", "Admin");
                }
            }
            else
            {
                return View(producer);
            }
        }
    }
}
