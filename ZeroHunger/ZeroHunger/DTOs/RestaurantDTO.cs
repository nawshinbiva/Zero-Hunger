using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZeroHunger.DTO
{
    public class RestaurantDTO
    {
        public int Restaurant_ID { get; set; }

        [Required(ErrorMessage = "Restaurant name is required")]
        [Display(Name = "Restaurant Name")]
        public string Restaurant_Name { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Contact number must be a 10-digit number")]
        [Display(Name = "Contact Number")]
        public string Contact_Number { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        
    }
}
