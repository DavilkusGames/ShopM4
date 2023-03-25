using System;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
	public interface IRepositoryOrderHeader : IRepository<OrderHeader>
    {
        void Update(OrderHeader obj);
    }
}

