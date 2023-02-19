using System;
using ShopM4_DataMigrations.Data;
using ShopM4_DataMigrations.Repository.IRepository;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository
{
    internal class RepositoryMyModel : Repository<MyModel>, IRepositoryMyModel
    {
        public RepositoryMyModel(ApplicationDbContext db) : base(db) { }

        public void Update(Category obj)
        {
            MyModel objFromDb = db.MyModel.FirstOrDefault(x => x.Id == obj.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = obj.Name;
                objFromDb.Number = obj.Number;
            }
        }
    }
}
