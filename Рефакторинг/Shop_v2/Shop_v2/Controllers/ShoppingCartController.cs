using Shop_v2.Context;
using Shop_v2.Models;
using Shop_v2.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Shop_v2.Controllers
{
    public class ShoppingCartController : Controller
    {
        Shop_version_2_Context storeDB = new Shop_version_2_Context();

        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new Models.ShoppingCartModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()

            };
            return View(viewModel);
        }

        public ActionResult AddToCart(int id)
        {
            var addedProd = storeDB.Products
                .Single(album => album.Id == id);

            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.AddToCart(addedProd);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            string albumName = storeDB.Carts
                .Single(item => item.RecordId == id).Product.Name;

            int itemCount = cart.RemoveFromCart(id);

            var data = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(albumName) +
                    " был удален из вашей корзины.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(data);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }

        [Authorize]
        public ActionResult Checkout(ShoppingCartModel cartModel)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            var viewModel = new Models.ShoppingCartModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public ActionResult OrderNextStep(string name)
        {
            var currentUser = storeDB.Users.FirstOrDefault(c => c.UserName == name);
            return View(currentUser);
        }

        public ActionResult ConfirmOrder()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart();
            return View();
        }
    }
}