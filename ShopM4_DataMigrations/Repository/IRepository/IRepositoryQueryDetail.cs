using System;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryQueryDetail : IRepository<QueryDetail>
    {
        void Update(QueryDetail obj);
    }
}

