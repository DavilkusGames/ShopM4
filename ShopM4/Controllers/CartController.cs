using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopM4.Data;
using ShopM4.Models;
using ShopM4.Utility;

namespace ShopM4.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext db;

        public CartController(ApplicationDbContext db)
        {
            this.db = db;
        }

        public IActionResult Index()
        {
            List<Cart> cartList = new List<Cart>();

            if (HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<Cart>>(PathManager.SessionCart).Count() > 0)
            {
                cartList = HttpContext.Session.Get<List<Cart>>(PathManager.SessionCart);

                // Хотим получить каждый товар из корзины
            }

            // Получаем лист id товаров
            List<int> productsIdInCard = cartList.Select(x => x.ProductId).ToList();

            // Извлекаем сами продукты по списку id
            IEnumerable<Product> productList = db.Product.Where(x => productsIdInCard.Contains(x.Id)).ToList();

            return View();
        }
    }
}
