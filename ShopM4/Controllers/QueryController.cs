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
            QueryViewModel = new QueryViewModel()
            {
                // извлекаем хедер из репозитория
                QueryHeader = repositoryQueryHeader.FirstOrDefault(x => x.Id == id),
                QueryDetail = repositoryQueryDetail.GetAll(x => x.QueryHeaderId == id,
                 includeProperties: "Product")
            };

            return View(QueryViewModel);
        }

        public IActionResult GetQueryList()
        {
            JsonResult result = Json(new { data = repositoryQueryHeader.GetAll() });

            return result;
        }
    }
}

