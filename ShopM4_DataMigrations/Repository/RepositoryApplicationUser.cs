using System;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository
{
    internal class RepositoryApplicationUser : Repository<ApplicationUser>, IRepositoryApplicationUser
    {
        public RepositoryApplicationUser(ApplicationDbContext db) : base(db) { }
    }
}
