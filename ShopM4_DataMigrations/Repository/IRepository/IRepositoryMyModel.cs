using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShopM4_Models;

namespace ShopM4_DataMigrations.Repository.IRepository
{
    public interface IRepositoryMyModel : IRepository<MyModel>
    {
        void Update(Category obj); 
    }
}
