using System;

namespace ShopM4_Models.ViewModels
{
	public class OrderHeaderDetailViewModel
	{
		public OrderHeader OrderHeader { get; set; }

		public IEnumerable<OrderDetail> OrderDetail { get; set; }
	}
}

