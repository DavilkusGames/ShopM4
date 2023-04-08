using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ShopM4_Models.ViewModels
{
	public class OrderViewModel
	{
        public IEnumerable<OrderHeader> OrderHeaderList { get; set; }

        // для выдападающего списка - статус
        public IEnumerable<SelectListItem> StatusList { get; set; }

        // текущее значение статуса
        public string Status { get; set; }
    }
}

