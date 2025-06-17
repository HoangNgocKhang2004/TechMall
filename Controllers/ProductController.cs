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

namespace TechMall.Controllers
{
    public class ProductController : Controller
    {
        WebAspDbEntities objWebAspDbEntities = new WebAspDbEntities();
        // GET: Product
        public async Task<ActionResult> Detail(int Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/");
                var response = await client.GetAsync($"api/products/{Id}");
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    var product = JsonConvert.DeserializeObject<ProductViewModel>(jsonString);
                    return View(product);
                }
                return HttpNotFound();
            }
        }

    }
}