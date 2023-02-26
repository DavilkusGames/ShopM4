using System;
using ShopM4_Models;


namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryMyModel : IRepository<MyModel>
    {
        void Update(MyModel obj);
    }
}