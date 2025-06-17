using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace TechMall.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = "";

        public string Image { get; set; } = "";

        public string ShortDes { get; set; } = "";

        public string FullDescription { get; set; } = "";

        public decimal Price { get; set; }

        public decimal? PriceDiscount { get; set; }

        public string CategoryName { get; set; } = "";

        public string BrandName { get; set; } = "";

        // Những trường dưới API không có, nhưng để tương thích UI có thể giữ lại với giá trị mặc định:
        public bool ShowOnHomePage { get; set; } = false;

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool Deleted { get; set; } = false;

        // Các danh sách phục vụ cho Dropdown nếu có
        public IEnumerable<SelectListItem> Categories { get; set; }

        public IEnumerable<SelectListItem> Brands { get; set; }
    }
}
