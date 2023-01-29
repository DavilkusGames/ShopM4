using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopM4_Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }

        public IEnumerable<SelectListItem> MyModelList { get; set; }
    }
}

