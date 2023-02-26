using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryProduct : IRepository<Product>
    {
        void Update(Product obj);

        IEnumerable<SelectListItem> GetListItems(string obj);
    }
}
