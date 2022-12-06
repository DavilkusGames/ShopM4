using System;
using Microsoft.AspNetCore.Mvc;
using ShopM4.Data;
using ShopM4.Models;

namespace ShopM4.Controllers
{
	public class MyModelController : Controller
	{
		private ApplicationDbContext db;

		public MyModelController(ApplicationDbContext db)
		{
			this.db = db;
		}

		public IActionResult Index()
		{
			IEnumerable<MyModel> models = db.MyModel;

			return View(models);
		}

		// GET - Create
		public IActionResult Create()
		{
			return View();
		}

		// POST - Create
		[HttpPost]
		public IActionResult Create(MyModel model)
		{
			if (model == null || model.Name == null)
				return RedirectToAction("Index");
			db.MyModels.Add(model);
			db.SaveChanges();

			return RedirectToAction("Index");  //переход на страницу Index
		}

		// GET - Edit
		[HttpGet]
		public IActionResult Edit(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var model = db.MyModels.Find(id);

			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		//POST - Edit
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult Edit(MyModel model)
		{
			if (ModelState.IsValid)
			{
				db.MyModel.Update(model);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(model);
		}

		// GET - Delete
		[HttpGet]
		public IActionResult Delete(int? id)
		{
			if (id == null || id == 0)
			{
				return NotFound();
			}

			var model = db.MyModels.Find(id);

			if (model == null)
			{
				return NotFound();
			}

			return View(model);
		}

		//POST - Delete
		[HttpPost]
		[ValidateAntiForgeryToken]
		public IActionResult DeletePost(int? id)
		{
			var model = db.MyModel.Find(id);

			if (model == null)
			{
				return NotFound();
			}

			db.MyModels.Remove(model);
			db.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}