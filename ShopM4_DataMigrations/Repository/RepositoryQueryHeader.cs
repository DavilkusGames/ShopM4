using System;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository
{
    public class RepositoryQueryHeader : Repository<QueryHeader>, IRepositoryQueryHeader
    {
        public RepositoryQueryHeader(ApplicationDbContext db) : base(db) { }

        public void Update(QueryHeader obj)
        {
            db.QueryHeader.Update(obj);
        }
    }
}

