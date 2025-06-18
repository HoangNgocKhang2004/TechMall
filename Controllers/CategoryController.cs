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
    public class CategoryController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoryController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080") // Thay bằng URL Spring Boot API của bạn
            };
        }

        public async Task<ActionResult> Index()
        {
            var response = await _httpClient.GetAsync("/api/categories");

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var listCategory = JsonConvert.DeserializeObject<List<Category>>(jsonString);

                var homeModel = new HomeModel
                {
                    ListCategory = listCategory
                };

                return View(homeModel);
            }
            else
            {
                ViewBag.Error = "Không thể tải danh sách danh mục.";
                return View(new HomeModel());
            }
        }
    }
}