using System;
using Microsoft.AspNetCore.Identity;

namespace ShopM4.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}

