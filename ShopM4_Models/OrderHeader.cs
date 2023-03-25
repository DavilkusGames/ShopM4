using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopM4_Models
{
	public class OrderHeader
	{
		[Key]
		public int Id { get; set; }

		// id admin
		public string AdminId { get; set; }
		[ForeignKey("AdminId")]
		public ApplicationUser Admin { get; set; }

		[Required]
		public DateTime DateOrder { get; set; }

        [Required]
        public double TotalPrice { get; set; }

		public string Status { get; set; }

        [Required]
        public string FullName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public string House { get; set; }
        [Required]
        public string Apartment { get; set; }
        [Required]
        public string PostalCode { get; set; }

    }
}

