using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ZeroHunger.DTOs;

namespace ZeroHunger.DTO
{
    public class CollectRequestDTO
    {
        public int Request_ID { get; set; }

        [Required(ErrorMessage = "Restaurant ID is required")]
        [Display(Name = "Restaurant ID")]
        public int Restaurant_ID { get; set; }

        [Required(ErrorMessage = "Requested Time is required")]
        [Display(Name = "Requested Time")]
        public DateTime Request_DateTime { get; set; }

        [Required(ErrorMessage = "Maximum preservation time is required")]
        [Display(Name = "Maximum Preservation Time (Hours)")]
        public int Maximum_Preservation_Time { get; set; }

        [Required(ErrorMessage = "Food item name is required")]
        [Display(Name = "Food Item Name")]
        public string FoodItem_Name { get; set; }

        [Required(ErrorMessage = "Food item quantity is required")]
        [Range(0, double.MaxValue, ErrorMessage = "Food item quantity must be greater than or equal to 0")]
        [Display(Name = "Food Item Quantity (Kg)")]
        public double FoodItem_Quantity { get; set; }

        
        [Display(Name = "Status")]
        public string Status { get; set; }

        
    }
}
