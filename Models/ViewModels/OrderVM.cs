using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechMall.Models.ViewModels
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Status { get; set; }
    }
}