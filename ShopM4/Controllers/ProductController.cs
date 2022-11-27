﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopM4.Data;
using ShopM4.Models;
using ShopM4.Models.ViewModels;

namespace ShopM4.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db;
        private IWebHostEnvironment webHostEnvironment;

        public ProductController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            this.db = db;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = db.Product;

            // Получаем ссылки на сущности категорий
            foreach (var item in objList)
            {
                item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }

            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {
            // Получаем лист категорий для отправки его во View
            IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x =>
            new SelectListItem
            {
                Text = x.Name,
                Value = id.ToString()
            });
            // Отправляем лист категорий во View
            //ViewBag.CategoriesList = CategoriesList;
            ViewData["CategoriesList"] = CategoriesList;

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };

            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                productViewModel.Product = db.Product.Find(id);

                if (productViewModel == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }

        // POST CreateEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateEdit(ProductViewModel productViewModel)
        {
            var files = HttpContext.Request.Form.Files;

            string wwwRoot = webHostEnvironment.WebRootPath;

            if (productViewModel.Product.Id == 0)
            {
                // create
                string upload = wwwRoot + PathManager.ImageProductPath;
                string imageName = Guid.NewGuid().ToString();

                string extension = Path.GetExtension(files[0].FileName);

                using (var FileStream = new FileStream("", FileMode.Create)) {

                }
            }
            else
            {
                // update
            }

            return View();
        }
    }
}
