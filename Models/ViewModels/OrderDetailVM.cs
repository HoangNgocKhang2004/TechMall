using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechMall.Models.ViewModels
{
    public class OrderDetailVM
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}