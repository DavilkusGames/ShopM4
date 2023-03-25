using System;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository
{
	public class RepositoryOrderDetail : Repository<OrderDetail>, IRepositoryOrderDetail
    {
        public RepositoryOrderDetail(ApplicationDbContext db) : base(db) { }

        public void Update(OrderDetail obj)
        {
            db.OrderDetail.Update(obj);
        }
    }
}