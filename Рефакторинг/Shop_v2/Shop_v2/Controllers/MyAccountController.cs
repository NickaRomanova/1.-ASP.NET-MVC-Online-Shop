using Shop_v2.Context;
using Shop_v2.Models;
using System;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;


namespace Shop_v2.Controllers
{
    public class MyAccountController : Controller
    {
        Shop_version_2_Context db = new Shop_version_2_Context();
        private string encryptedPassword;
        private string decryptPassword;
        private string hash = "f0xlern";

        private void Encrypt(string password)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    encryptedPassword = Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        private void Decyrpt(string password)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    decryptPassword = Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        [Authorize]
        public ActionResult MyProfile(string name)
        {
            var currentUser = db.Users.FirstOrDefault(c => c.UserName == name);
            return View(currentUser);
        }

        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Registration(User user, string ReturnUrl = "")
        {
            if (ModelState.IsValid)
            {

                var item = db.Users.FirstOrDefault(c => c.UserName == user.UserName);
                var itemEmail = db.Users.FirstOrDefault(c => c.Email == user.Email);
                if (item != null)
                {
                    ViewBag.Message = "Такой пользователь уже есть. Введите уникальное имя пользователя.";
                    return View();
                }
                else
                {
                    if (itemEmail != null)
                    {
                        ViewBag.Message = "Пользователь с таким email уже существует. Воспользуетесь другим email.";
                        return View();
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(user.UserName, true);

                        if (Url.IsLocalUrl(ReturnUrl))
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            Encrypt(user.Password);
                            user.Password = encryptedPassword;
                            user.RePassword = encryptedPassword;

                            db.Users.Add(user);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
            }
            else
            {
                return View(user);
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnUrl = "")
        {
            using (Shop_version_2_Context dc = new Shop_version_2_Context())
            {
                Decyrpt(l.Password);
                l.Password = decryptPassword;

                var user = dc.Users.Where(a => a.UserName.Equals(l.UserName) && a.Password.Equals(l.Password)).FirstOrDefault();

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.UserName, l.RememberMe);

                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }
            ViewBag.Message = "Неправильное имя пользователя или пароль";
            ModelState.Remove("Password");
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.EmptyCart();

            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditUser(string name)
        {
            var currentUser = db.Users.FirstOrDefault(c => c.UserName == name);

            Decyrpt(currentUser.Password);
            currentUser.Password = decryptPassword;

            return View(currentUser);
        }

        [Authorize]
        [HttpGet]
        public ActionResult EditUser(int id)
        {
            var currentUser = db.Users.FirstOrDefault(c => c.Id == id);

            Decyrpt(currentUser.Password);
            currentUser.Password = decryptPassword;

            return View(currentUser);
        }

        [HttpPost]
        public ActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                Encrypt(user.Password);
                user.Password = encryptedPassword;
                user.RePassword = encryptedPassword;
                db.SaveChanges();
                return RedirectToAction("Login", "MyAccount");
            }
            else
            {
                return View(user);
            }
        }

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPasswordRe(string email)
        {
            var currentUser = db.Users.FirstOrDefault(c => c.Email == email);

            if (currentUser != null)
            {
                byte[] data = Convert.FromBase64String(currentUser.Password);
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                        currentUser.Password = UTF32Encoding.UTF8.GetString(results);
                    }
                }

                MailAddress fromMailAddress = new MailAddress("nickaromanova1998@gmail.com", "Nicka Romanova");
                MailAddress toAddress = new MailAddress(email, "User");


                using (MailMessage mailMessage = new MailMessage(fromMailAddress, toAddress))
                using (SmtpClient smtpClient = new SmtpClient())
                {

                    mailMessage.Subject = "Восстановление пароля от онлайн магазина детских товаров Львенок ";
                    mailMessage.Body = "Ваш логин: " + currentUser.UserName + "\n" + "Ваш пароль: " + currentUser.Password + "\n\n" + "Спасибо, что используете Львенка";

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = new System.Net.NetworkCredential(fromMailAddress.Address, "kallesin");

                    smtpClient.Send(mailMessage);
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }

        }
    }
}