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

            return RedirectToAction("Index", "Cart");
        }

        public IActionResult GetQueryList()
        {
            JsonResult result = Json(new { data = repositoryQueryHeader.GetAll() });

            return result;
        }
    }
}

