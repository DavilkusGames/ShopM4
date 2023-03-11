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
    [Authorize(Roles = PathManager.AdminRole)]
    public class QueryController : Controller
    {
        private IRepositoryQueryHeader repositoryQueryHeader;
        private IRepositoryQueryDetail repositoryQueryDetail;

        [BindProperty]
        public QueryViewModel QueryViewModel { get; set; }

        public QueryController(IRepositoryQueryHeader repositoryQueryHeader,
            IRepositoryQueryDetail repositoryQueryDetail)
        {
            this.repositoryQueryDetail = repositoryQueryDetail;
            this.repositoryQueryHeader = repositoryQueryHeader;
        }

        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult Details(int id)
        {
            QueryViewModel = new QueryViewModel()
            {
                // извлекаем хедер из репозитория
                QueryHeader = repositoryQueryHeader.FirstOrDefault(x => x.Id == id),
                QueryDetail = repositoryQueryDetail.GetAll(x => x.QueryHeaderId == id,
                 includeProperties: "Product")
            };

            return View(QueryViewModel);
        }

        [HttpPost]
        public IActionResult Details()
        {
            List<Cart> carts = new List<Cart>();

            QueryViewModel.QueryDetail = repositoryQueryDetail.GetAll(
                x => x.QueryHeader.Id == QueryViewModel.QueryHeader.Id);

            // создаем корзину покупок и добавляем значения в сессию
            foreach (var item in QueryViewModel.QueryDetail)
            {
                Cart cart = new Cart() { ProductId = item.ProductId };

                carts.Add(cart);
            }

            // работа с сессиями
            HttpContext.Session.Clear();
            HttpContext.Session.Set(PathManager.SessionCart, carts);

            // создаем еще одну сессию для определения того, что мы изменяем заказ
            HttpContext.Session.Set(PathManager.SessionQuery, QueryViewModel.QueryHeader.Id);

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            QueryHeader queryHeader = repositoryQueryHeader.FirstOrDefault(
                x => x.Id == QueryViewModel.QueryHeader.Id);

            // получаем детали запроса
            IEnumerable<QueryDetail> queryDetails = repositoryQueryDetail.GetAll(
                x => x.QueryHeaderId == QueryViewModel.QueryHeader.Id);


            repositoryQueryDetail.Remove(queryDetails);
            repositoryQueryHeader.Remove(queryHeader);

            repositoryQueryHeader.Save();

            return RedirectToAction("Index");
        }

        public IActionResult GetQueryList()
        {
            JsonResult result = Json(new { data = repositoryQueryHeader.GetAll() });

            return result;
        }
    }
}

