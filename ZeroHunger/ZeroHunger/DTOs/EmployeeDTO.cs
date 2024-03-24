using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZeroHunger.DTOs;

namespace ZeroHunger.DTO
{
    public class EmployeeDTO
    {
        public int Employee_ID { get; set; }

        [Required(ErrorMessage = "Employee name is required")]
        [Display(Name = "Employee Name")]
        public string Employee_Name { get; set; }

        [Required(ErrorMessage = "Contact number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Contact number must be a 11-digit number")]
        [Display(Name = "Contact Number")]
        public string Contact_Number { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

    }
}
