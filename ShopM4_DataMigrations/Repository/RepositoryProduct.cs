using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

using ShopM4_Utility;


namespace ShopM4_DataMigrations.Repository
{
    public class RepositoryProduct : Repository<Product>, IRepositoryProduct
    {
        public RepositoryProduct(ApplicationDbContext db) : base(db) { }

        public IEnumerable<SelectListItem> GetListItems(string obj)
        {
            if (obj == PathManager.NameCategory)
            {
                return db.Category.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }

            if (obj == PathManager.NameMyModel)
            {
                return db.MyModel.Select(x =>
                new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                });
            }

            return null;
        }

        public void Update(Product obj)
        {
            db.Update(obj);   // !!! check !!!
        }
    }
}

