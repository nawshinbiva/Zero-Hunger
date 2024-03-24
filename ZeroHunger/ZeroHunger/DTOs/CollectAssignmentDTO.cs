using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs
{
    public class CollectAssignmentDTO
    {
        public int Assignment_ID { get; set; }
        public int Request_ID { get; set; }
        public int Employee_ID { get; set; }
        public DateTime Assignment_DateTime { get; set; }
        public string Status { get; set; }
    }
}