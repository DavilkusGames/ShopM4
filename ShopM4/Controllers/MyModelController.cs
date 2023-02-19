using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using ShopM4_DataMigrations.Data;
using ShopM4_Models;
using ShopM4_Utility;
using ShopM4_DataMigrations.Repository.IRepository;

namespace ShopM4.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        //private ApplicationDbContext db;
        private IRepositoryMyModel repositoryMyModel;

        public MyModelController(IRepositoryMyModel repositoryMyModel)
        {
            this.repositoryMyModel = repositoryMyModel;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<MyModel> models = db.MyModel;

            return View(models);
        }
    }
}

