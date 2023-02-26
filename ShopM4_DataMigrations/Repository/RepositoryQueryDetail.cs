using System;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository
{
    public class RepositoryQueryDetail : Repository<QueryDetail>, IRepositoryQueryDetail
    {
        public RepositoryQueryDetail(ApplicationDbContext db) : base(db) { }

        public void Update(QueryDetail obj)
        {
            db.QueryDetail.Update(obj);
        }
    }
}
