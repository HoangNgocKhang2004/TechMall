using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TechMall.Context;
using TechMall.Models;

namespace TechMall.Controllers
{
    public class HomeController : Controller
    {
        WebAspDbEntities objWebAspDbEntities = new WebAspDbEntities();
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

        public ActionResult Large(int Id, int page = 1, int pageSize = 6)
        {
            List<Product> listProduct;

            if (Id == 0) // Nếu ID là 0, lấy tất cả sản phẩm
            {
                listProduct = objWebAspDbEntities.Products.Where(b => b.ShowOnHomePage.HasValue && b.ShowOnHomePage.Value == true).ToList();
            }
            else
            {
                listProduct = objWebAspDbEntities.Products.Where(b => b.CategoryId == Id && b.ShowOnHomePage.HasValue && b.ShowOnHomePage.Value == true).ToList();
            }

            // Tính tổng số sản phẩm
            int totalProducts = listProduct.Count;

            // Phân trang
            var paginatedProducts = listProduct.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            // Truyền dữ liệu cần thiết sang ViewBag
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

                    // Gán mặc định
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

                    // Lọc theo tên danh mục nếu có
                    if (!string.IsNullOrEmpty(categoryName))
                    {
                        allProducts = allProducts
                            .Where(p => string.Equals(p.CategoryName, categoryName, StringComparison.OrdinalIgnoreCase))
                            .ToList();
                    }

                    // Lọc sản phẩm chưa bị xóa
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


        //GET: Register

        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User _user)
        {
            if (ModelState.IsValid)
            {
                var check = objWebAspDbEntities.Users.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    _user.IsAdmin = false;
                    _user.CreatedAt = DateTime.Now;
                    objWebAspDbEntities.Configuration.ValidateOnSaveEnabled = false;
                    objWebAspDbEntities.Users.Add(_user);
                    objWebAspDbEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email đã tồn tại!";
                    return View();
                }


            }
            return View();


        }
        //create a string MD5
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");

            }
            return byte2String;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {


                var f_password = GetMD5(password);
                var data = objWebAspDbEntities.Users.Where(s => s.Email.Equals(email) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["FullName"] = data.FirstOrDefault().FirstName + " " + data.FirstOrDefault().LastName;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["idUser"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Đăng nhập thất bại";
                    return RedirectToAction("Login");
                }
            }
            return View();
        }


        //Logout
        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult HandleFirebaseLogin(FirebaseUserModel model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra người dùng trong database hoặc tạo mới
                var user = objWebAspDbEntities.Users.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    user = new User
                    {
                        DisplayName = model.DisplayName,
                        Email = model.Email,
                        Provider = model.Provider,
                        FirebaseUid = model.Uid
                    };
                    objWebAspDbEntities.Users.Add(user);
                    objWebAspDbEntities.SaveChanges();
                }

                // Lưu thông tin đăng nhập vào session
                Session["UserId"] = user.Id;
                Session["UserEmail"] = user.Email;
                Session["UserName"] = user.DisplayName;

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpGet]
        public JsonResult CheckLoginStatus()
        {
            bool isLoggedIn = Session["idUser"] != null;
            return Json(isLoggedIn, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LiveSearch(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return Content(""); // Nếu không có từ khóa, trả về rỗng
            }

            // Tìm kiếm trong database
            var results = objWebAspDbEntities.Products
                .Where(p => p.Name.Contains(query) || p.ShortDes.Contains(query))
                .Take(10) // Giới hạn số kết quả (ví dụ: 10 kết quả)
                .ToList();

            // Trả về một PartialView chứa danh sách kết quả
            return PartialView("_SearchResults", results);
        }
        private WebAspDbEntities _context;

        public HomeController()
        {
            _context = new WebAspDbEntities();
        }

        // Action trả về danh sách danh mục dưới dạng JSON
        [HttpGet]
        public ActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
    }
}