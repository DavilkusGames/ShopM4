using System;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using ShopM4.Data;
using ShopM4_Models;
using ShopM4_Utility;

namespace ShopM4.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class MyModelController : Controller
    {
        private ApplicationDbContext db;

        public MyModelController(ApplicationDbContext db)
        {
            this.db = db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            IEnumerable<MyModel> models = db.MyModel;

            return View(models);
        }
    }
}

