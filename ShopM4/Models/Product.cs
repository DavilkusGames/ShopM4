using System;
using ShopM4.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopM4.Models
{
	public class Product
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public string Description { get; set; }

		public string ShortDescription { get; set; }

		[Required]
		[Range(1, int.MaxValue, ErrorMessage = "Value must be more than 1.")]
		public double Price { get; set; }

		public string Image { get; set; }

		// явное добавление представления для внешнего ключа
		[Display(Name = "Category Id")]
		public int CategoryId { get; set; }

		// добавление внешнего ключа
		[ForeignKey("CategoryId")]
		public virtual Category Category { get; set; }

		[Display(Name = "MyModel Id")]
		public int MyModelId { get; set; }

		[ForeignKey("MyModelId")]
		public virtual MyModel MyModel { get; set; }
	}
}

