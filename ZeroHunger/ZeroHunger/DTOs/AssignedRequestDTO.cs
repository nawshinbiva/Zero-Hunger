using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTO
{
    public class AssignedRequestDTO
    {
        public int Request_ID { get; set; }
        public int Restaurant_ID { get; set; }
        public DateTime Request_DateTime { get; set; }
        public int Maximum_Preservation_Time { get; set; }
        public string FoodItem_Name { get; set; }
        public double FoodItem_Quantity { get; set; }
        public string Status { get; set; }
        // Add more properties as needed
    }
}