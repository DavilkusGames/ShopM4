using System;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository
{
    public class RepositoryCategory : Repository<Category>, IRepositoryCategory
    {
        public RepositoryCategory(ApplicationDbContext db) : base(db) { }

        public void Update(Category obj)
        {
            var objFromDb = db.Category.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.DisplayOrder = obj.DisplayOrder;
            }
        }
    }
}

