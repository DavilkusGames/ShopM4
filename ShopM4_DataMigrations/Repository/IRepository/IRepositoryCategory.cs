using System;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryCategory : IRepository<Category>
    {
        void Update(Category obj);
    }
}

