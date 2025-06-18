using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechMall.Context;
using TechMall.Models;
using TechMall.Models.ViewModels;

namespace TechMall.Controllers
{
    public class HomeController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:8080/api/";
        private readonly HttpClient _httpClient;

        public HomeController()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://localhost:8080") // Thay bằng API URL thật sự
            };
        }
        //WebAspDbEntities objWebAspDbEntities = new WebAspDbEntities();
        public async Task<ActionResult> Index()
        {
            var model = new HomeModel
            {
                ListProduct = new List<ProductViewModel>(),
                ListCategory = new List<Category>(),
                ListBrand = new List<Brand>()
            };

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/");

                var productRes = await client.GetAsync("api/products");
                if (productRes.IsSuccessStatusCode)
                {
                    string json = await productRes.Content.ReadAsStringAsync();
                    model.ListProduct = JsonConvert.DeserializeObject<List<ProductViewModel>>(json);
                }

                var cateRes = await client.GetAsync("api/categories");
                if (cateRes.IsSuccessStatusCode)
                {
                    string json = await cateRes.Content.ReadAsStringAsync();
                    model.ListCategory = JsonConvert.DeserializeObject<List<Category>>(json);
                }

                var brandRes = await client.GetAsync("api/brands");
                if (brandRes.IsSuccessStatusCode)
                {
                    string json = await brandRes.Content.ReadAsStringAsync();
                    model.ListBrand = JsonConvert.DeserializeObject<List<Brand>>(json);
                }
            }

            return View("Index", model); // RÕ RÀNG là View Index + Model HomeModel
        }

        public async Task<ActionResult> Large(int Id, int page = 1, int pageSize = 6)
        {
            var response = await _httpClient.GetAsync("/api/products");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Không thể tải danh sách sản phẩm.";
                return View(new List<Product>());
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var allProducts = JsonConvert.DeserializeObject<List<Product>>(jsonString);

            // Lọc theo CategoryId và ShowOnHomePage == true
            var filteredProducts = allProducts
                .Where(p => (Id == 0 || p.CategoryId == Id) && p.ShowOnHomePage == true)
                .ToList();

            // Tổng số sản phẩm
            int totalProducts = filteredProducts.Count;

            // Phân trang
            var paginatedProducts = filteredProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Truyền ViewBag
            ViewBag.CurrentCategoryId = Id;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.PageSize = pageSize;

            return View(paginatedProducts);
        }


        public async Task<ActionResult> Grid(string categoryName, int page = 1, int pageSize = 8)
        {
            List<ProductViewModel> allProducts = new List<ProductViewModel>();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080/");
                HttpResponseMessage response = await client.GetAsync("api/products");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    allProducts = JsonConvert.DeserializeObject<List<ProductViewModel>>(json);

                    foreach (var p in allProducts)
                    {
                        p.Deleted = p?.Deleted ?? false;
                        p.ShowOnHomePage = p?.ShowOnHomePage ?? false;
                        p.CategoryName = p.CategoryName ?? "";
                        p.BrandName = p.BrandName ?? "";
                        p.Image = p.Image ?? "";
                        p.ShortDes = p.ShortDes ?? "";
                        p.FullDescription = p.FullDescription ?? "";
                    }

                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        allProducts = allProducts
                            .Where(p => string.Equals(p.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }

                    allProducts = allProducts.Where(p => !p.Deleted).ToList();
                }
            }

            int totalProducts = allProducts.Count;
            var paginatedProducts = allProducts
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.CurrentCategoryId = categoryName;
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.PageSize = pageSize;

            return View(paginatedProducts); // View dùng @model List<ProductViewModel>
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(UserVM model)
        {
            if (!ModelState.IsValid)
                return View();

            model.Admin = false;
            model.CreatedAt = DateTime.Now;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080");

                int newId;
                bool idExists;

                do
                {
                    newId = new Random().Next(10000, 99999);

                    var checkResponse = await client.GetAsync($"/api/users/{newId}");
                    idExists = checkResponse.IsSuccessStatusCode;
                }
                while (idExists);

                model.Id = newId;

                model.Password = model.Password;
                System.Diagnostics.Debug.WriteLine("Password gửi đi là: " + model.Password);


                var json = JsonConvert.SerializeObject(model);
                System.Diagnostics.Debug.WriteLine("Dữ liệu JSON gửi đi: " + json);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("/api/users", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.error = "Đăng ký thất bại hoặc email đã tồn tại";
                    return View();
                }
            }
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (!ModelState.IsValid)
                return View();

            //var hashedPassword = GetMD5(password);
            var loginData = new { email = email, password = password };
            var json = JsonConvert.SerializeObject(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.PostAsync("http://localhost:8080/api/users/login", content);
                var responseData = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("API RESPONSE: " + responseData);

                if (response.IsSuccessStatusCode)
                {
                    var user = JsonConvert.DeserializeObject<UserVM>(responseData);
                    Session["FullName"] = user.FirstName + " " + user.LastName;
                    Session["Email"] = user.Email;
                    Session["idUser"] = user.Id;
                    return RedirectToAction("Index");
                }

                ViewBag.error = "Đăng nhập thất bại";
                return View();
            }
        }


        private string GetMD5(string str)
        {
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(str);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (var b in hashBytes)
                    sb.Append(b.ToString("X2"));

                return sb.ToString().ToLower();
            }
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> HandleFirebaseLogin(FirebaseUserModel model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false });

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080");

                // Kiểm tra xem user đã tồn tại chưa
                var check = await client.GetAsync($"/api/users/email/{model.Email}");
                User user;

                if (check.IsSuccessStatusCode)
                {
                    var json = await check.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(json);
                }
                else
                {
                    // Nếu chưa có, tạo mới
                    var newUser = new User
                    {
                        DisplayName = model.DisplayName,
                        Email = model.Email,
                        Provider = model.Provider,
                        FirebaseUid = model.Uid
                    };

                    var json = JsonConvert.SerializeObject(newUser);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("/api/users", content);

                    if (!response.IsSuccessStatusCode)
                        return Json(new { success = false });

                    var createdJson = await response.Content.ReadAsStringAsync();
                    user = JsonConvert.DeserializeObject<User>(createdJson);
                }

                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.DisplayName;

                return Json(new { success = true });
            }
        }


        [HttpGet]
        public JsonResult CheckLoginStatus()
        {
            bool isLoggedIn = Session["UserId"] != null;
            return Json(isLoggedIn, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> LiveSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
                return Content("");

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080");
                var response = await client.GetAsync($"/api/products/search?query={Uri.EscapeDataString(query)}");

                if (!response.IsSuccessStatusCode)
                    return Content("Không thể tìm kiếm.");

                var json = await response.Content.ReadAsStringAsync();
                var results = JsonConvert.DeserializeObject<List<Product>>(json);

                return PartialView("_SearchResults", results);
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetCategories()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:8080");
                var response = await client.GetAsync("/api/categories");

                if (!response.IsSuccessStatusCode)
                    return Json(new List<Category>(), JsonRequestBehavior.AllowGet);

                var json = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<Category>>(json);

                return Json(categories, JsonRequestBehavior.AllowGet);
            }
        }

    }
}