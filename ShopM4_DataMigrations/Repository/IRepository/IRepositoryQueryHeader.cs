using System;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryQueryHeader : IRepository<QueryHeader>
    {
        void Update(QueryHeader obj);
    }
}

