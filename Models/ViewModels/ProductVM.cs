using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TechMall.Models.ViewModels
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }
        public string Image { get; set; }
        public string ShortDes { get; set; }
        public string FullDescription { get; set; }
        public double Price { get; set; }
        public double? PriceDiscount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; }

        public string CategoryName { get; set; }
        public string BrandName { get; set; }
    }

}