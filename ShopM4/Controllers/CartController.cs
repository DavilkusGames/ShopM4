using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using ShopM4_DataMigrations.Data;
using ShopM4_Models;
using ShopM4_Models.ViewModels;
using ShopM4_Utility;
using ShopM4_DataMigrations.Repository.IRepository;

namespace ShopM4.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        // ApplicationDbContext db;

        ProductUserViewModel productUserViewModel;

        IWebHostEnvironment webHostEnvironment;
        IEmailSender emailSender;

        IRepositoryProduct repositoryProduct;
        IRepositoryApplicationUser repositoryApplicationUser;

        IRepositoryQueryHeader repositoryQueryHeader;
        IRepositoryQueryDetail repositoryQueryDetail;

        public CartController(IWebHostEnvironment webHostEnvironment,
            IEmailSender emailSender, IRepositoryProduct repositoryProduct,
            IRepositoryApplicationUser repositoryApplicationUser,
            IRepositoryQueryHeader repositoryQueryHeader, IRepositoryQueryDetail repositoryQueryDetail)
        {
            this.webHostEnvironment = webHostEnvironment;
            this.emailSender = emailSender;
            this.repositoryApplicationUser = repositoryApplicationUser;
            this.repositoryProduct = repositoryProduct;
            this.repositoryQueryHeader = repositoryQueryHeader;
            this.repositoryQueryDetail = repositoryQueryDetail;
        }


        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);

                // хотим получить каждый товар из корзины
            }

            // получаем лист id товаров
            List<int> productsIdInCart = cartList.Select(x => x.ProductId).ToList();

            // извлекаем сами продукты по списку id
            //IEnumerable<Product> productList = db.Product.Where(x => productsIdInCart.Contains(x.Id));
            IEnumerable<Product> productList =
                repositoryProduct.GetAll(x => productsIdInCart.Contains(x.Id));

            return View(productList);
        }

        public IActionResult Remove(int id)
        {
            // удаление из корзины
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }

            cartList.Remove(cartList.FirstOrDefault(x => x.ProductId == id));

            // переназначение сессии
            HttpContext.Session.Set(PathManager.SessionCart, cartList);

            return RedirectToAction("Index");
        }

        public IActionResult InquiryConfirmation()
        {
            HttpContext.Session.Clear();   // очистить полностью сессию

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SummaryPost(ProductUserViewModel productUserViewModel)
        {
            // work with user
            var identityClaims = (ClaimsIdentity)User.Identity;
            var claim = identityClaims.FindFirst(ClaimTypes.NameIdentifier);
           
            // код для отправки сообщения
            // combine
            var path = webHostEnvironment.WebRootPath + Path.DirectorySeparatorChar.ToString() +
                "templates" + Path.DirectorySeparatorChar.ToString() + "Inquiry.html";

            var subject = "New Order";

            string bodyHtml = "";

            using (StreamReader reader = new StreamReader(path))
            {
                bodyHtml = reader.ReadToEnd();
            }

            string textProducts = "";
            foreach (var item in productUserViewModel.ProductList)
            {
                textProducts += $"- Name: {item.Name}, ID: {item.Id}\n";
            }

            string body = string.Format(bodyHtml, productUserViewModel.ApplicationUser.FullName,
                productUserViewModel.ApplicationUser.Email,
                productUserViewModel.ApplicationUser.PhoneNumber,
                textProducts
            );

            await emailSender.SendEmailAsync(productUserViewModel.ApplicationUser.Email, subject, body);
            await emailSender.SendEmailAsync("viosagmir@gmail.com", subject, body);

            // добавление данных в БД по заказу
            QueryHeader queryHeader = new QueryHeader()
            {
                ApplicationUserId = claim.Value,
                QueryDate = DateTime.Now,
                FullName = productUserViewModel.ApplicationUser.FullName,
                PhoneNumber = productUserViewModel.ApplicationUser.PhoneNumber,
                Email = productUserViewModel.ApplicationUser.Email,
                ApplicationUser = repositoryApplicationUser.FirstOrDefault(x => x.Id == claim.Value)
            };

            // получение юзера
            //var claimsIdentiry = (ClaimsIdentity)User.Identity;
            //var claim = claimsIdentiry.FindFirst(ClaimTypes.NameIdentifier);
            // claim.Value -> getId

            repositoryQueryHeader.Add(queryHeader);
            repositoryQueryHeader.Save();


            // сделать запись деталей - всех продуктов в БД
            foreach (var item in productUserViewModel.ProductList)
            {
                QueryDetail queryDetail = new QueryDetail()
                {
                    ProductId = item.Id,
                    QueryHeaderId = queryHeader.Id,
                    QueryHeader = queryHeader,
                    Product = repositoryProduct.Find(item.Id)
                };

                repositoryQueryDetail.Add(queryDetail);
            }
            repositoryQueryDetail.Save();


            return RedirectToAction("InquiryConfirmation");
        }


        [HttpPost]
        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;

            // если пользователь вошел в систему, то объект будет определен
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);
            }

            // получаем лист id товаров
            List<int> productsIdInCart = cartList.Select(x => x.ProductId).ToList();

            // извлекаем сами продукты по списку id
            //IEnumerable<Product> productList = db.Product.Where(x => productsIdInCart.Contains(x.Id));
            IEnumerable<Product> productList = repositoryProduct.GetAll(x => productsIdInCart.Contains(x.Id));


            productUserViewModel = new ProductUserViewModel()
            {
                //ApplicationUser = db.ApplicationUser.FirstOrDefault(x => x.Id == claim.Value),
                ApplicationUser = repositoryApplicationUser.FirstOrDefault(x => x.Id == claim.Value),
                ProductList = productList.ToList()
            };

            return View(productUserViewModel);
        }
    }
}

