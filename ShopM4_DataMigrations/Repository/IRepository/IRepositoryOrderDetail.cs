using System;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryOrderDetail : IRepository<OrderDetail>
    {
        void Update(OrderDetail obj);
    }
}