using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;    // !!!
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ShopM4_DataMigrations.Data;
using ShopM4_Models;
using ShopM4_Models.ViewModels;
using ShopM4_Utility;

using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_DataMigrations.Repository;

namespace ShopM4.Controllers
{
    [Authorize(Roles = PathManager.AdminRole)]
    public class ProductController : Controller
    {
        // private ApplicationDbContext db;

        private IRepositoryProduct repositoryProduct;

        private IWebHostEnvironment webHostEnvironment;

        public ProductController(IRepositoryProduct repositoryProduct,
            IWebHostEnvironment webHostEnvironment)
        {
            this.repositoryProduct = repositoryProduct;
            this.webHostEnvironment = webHostEnvironment;
        }

        // GET INDEX
        public IActionResult Index()
        {
            IEnumerable<Product> objList = repositoryProduct.GetAll();

            // получаем ссылки на сущности категорий
            /*
            foreach (var item in objList)
            {
                // сопоставление таблицы категорий и таблицы product
                item.Category = db.Category.FirstOrDefault(x => x.Id == item.CategoryId);
            }
            */

            return View(objList);
        }

        // GET - CreateEdit
        public IActionResult CreateEdit(int? id)
        {
            /*
            // получаем лист категорий для отправки его во View
            IEnumerable<SelectListItem> CategoriesList = db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });

            // отправляем лист категорий во View
            //ViewBag.CategoriesList = CategoriesList;
            ViewData["CategoriesList"] = CategoriesList;
            */

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Product = new Product(),
                CategoriesList = repositoryProduct.GetListItems(PathManager.NameCategory),
                MyModelList = repositoryProduct.GetListItems(PathManager.NameMyModel)
            };

            if (id == null)
            {
                // create product
                return View(productViewModel);
            }
            else
            {
                // edit product
                //productViewModel.Product = db.Product.Find(id);
                productViewModel.Product = repositoryProduct.Find(id.GetValueOrDefault());

                if (productViewModel.Product == null)
                {
                    return NotFound();
                }
                return View(productViewModel);
            }
        }


        // POST - CreateEdit
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

                string path = upload + imageName + extension;

                // скопируем файл на сервер
                using (var fileStream = new FileStream(path,FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                productViewModel.Product.Image = imageName + extension;

                //db.Product.Add(productViewModel.Product);
                repositoryProduct.Add(productViewModel.Product);
            }
            else
            {
                // update
                // AsNoTracking() - IMPORTANT!!!
                // var product = db.Product.AsNoTracking().
                //     FirstOrDefault(u => u.Id == productViewModel.Product.Id); // LINQ


                var product = repositoryProduct.
                    FirstOrDefault(u => u.Id == productViewModel.Product.Id, isTracking: false);

                if (files.Count > 0)  // юзер загружает другой файл
                {
                    string upload = wwwRoot + PathManager.ImageProductPath;
                    string imageName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);
                    string path = upload + imageName + extension;

                    // delete old file
                    var oldFile = upload + product.Image;

                    if (System.IO.File.Exists(oldFile))
                    {
                        System.IO.File.Delete(oldFile);
                    }

                    // скопируем файл на сервер
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productViewModel.Product.Image = imageName + extension;
                }
                else   // фотка не поменялась
                {
                    productViewModel.Product.Image = product.Image;  // оставляем имя прежним
                }

                //db.Product.Update(productViewModel.Product);
                repositoryProduct.Update(productViewModel.Product);
            }

            // db.SaveChanges();
            repositoryProduct.Save();

            return RedirectToAction("Index");

            //return View();
        }


        // GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //Product product = db.Product.Find(id);
            Product product = repositoryProduct.FirstOrDefault
                (x => x.Id == id, includeProperties: "Category,MyModel");    // ???

            if (product == null)
            {
                return NotFound();
            }

            // product.Category = db.Category.Find(product.CategoryId);
            // product.Category = db.Category.Find(product.CategoryId);

            return View(product);
        }

        // POST
        [HttpPost]
        public IActionResult DeletePost(int? id)
        {
            Product product = repositoryProduct.Find(id.GetValueOrDefault());

            if (product == null)
            {
                return NotFound();
            }

            // delete image from server
            string upload = webHostEnvironment.WebRootPath + PathManager.ImageProductPath;

            // получаем ссылку на нашу старую фотку
            var oldFile = upload + product.Image;

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            repositoryProduct.Remove(product);
            repositoryProduct.Save();

            return RedirectToAction("Index");
        }

    }
}

