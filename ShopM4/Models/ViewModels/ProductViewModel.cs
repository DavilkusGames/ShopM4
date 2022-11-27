﻿using System;
using System.Collections;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopM4.Models.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> CategoriesList { get; set; }
    }
}
