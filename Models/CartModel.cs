using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechMall.Context;
using TechMall.Models.ViewModels;

namespace TechMall.Models
{
    public class CartModel
    {
        public ProductVM Product { get; set; }
        public int Quantity { get; set; }
    }

}