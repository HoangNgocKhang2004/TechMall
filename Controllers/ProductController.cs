using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechMall.Context;
using TechMall.Models;
using TechMall.Models.ViewModels;

namespace TechMall.Controllers
{
    public class ProductController : Controller
    {
        WebAspDbEntities objWebAspDbEntities = new WebAspDbEntities();
        // GET: Product
        public async Task<ActionResult> Detail(int Id)
        {
            ProductVM product = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/");

                var response = await client.GetAsync($"api/products/{Id}");
                if (!response.IsSuccessStatusCode)
                {
                    return HttpNotFound();
                }

                var jsonString = await response.Content.ReadAsStringAsync();
                product = JsonConvert.DeserializeObject<ProductVM>(jsonString);

                // Lấy thông tin danh mục
                if (product.CategoryId.HasValue)
                {
                    var categoryRes = await client.GetAsync($"api/categories/{product.CategoryId.Value}");
                    if (categoryRes.IsSuccessStatusCode)
                    {
                        var categoryJson = await categoryRes.Content.ReadAsStringAsync();
                        var category = JsonConvert.DeserializeObject<CategoryViewModel>(categoryJson);
                        product.CategoryName = category?.Name;
                    }
                }

                // Lấy thông tin thương hiệu
                if (product.BrandId.HasValue)
                {
                    var brandRes = await client.GetAsync($"api/brands/{product.BrandId.Value}");
                    if (brandRes.IsSuccessStatusCode)
                    {
                        var brandJson = await brandRes.Content.ReadAsStringAsync();
                        var brand = JsonConvert.DeserializeObject<BrandViewModel>(brandJson);
                        product.BrandName = brand?.Name;
                    }
                }
            }

            return View(product);
        }


    }
}