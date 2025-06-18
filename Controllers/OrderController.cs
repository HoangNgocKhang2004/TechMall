using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using TechMall.Models.ViewModels;

namespace TechMall.Controllers
{
    public class OrderController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:8080/api/";

        public async Task<ActionResult> Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            int userId = (int)Session["idUser"];
            List<OrderVM> orders = new List<OrderVM>();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiBaseUrl + $"orders/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    orders = JsonConvert.DeserializeObject<List<OrderVM>>(json);
                }
            }

            Session["odercount"] = orders.Count;
            return View(orders);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrderDetails(int id)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiBaseUrl + $"orderdetails/order/{id}");
                if (!response.IsSuccessStatusCode)
                    return Content("Không tìm thấy chi tiết đơn hàng.");

                var json = await response.Content.ReadAsStringAsync();
                var orderDetails = JsonConvert.DeserializeObject<List<OrderDetailVM>>(json);

                return PartialView("_OrderDetailsPartial", orderDetails);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetOrderCount()
        {
            if (Session["idUser"] == null)
                return Json(new { count = 0 }, JsonRequestBehavior.AllowGet);

            int userId = (int)Session["idUser"];
            int count = 0;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiBaseUrl + $"orders/user/{userId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var orders = JsonConvert.DeserializeObject<List<OrderVM>>(json);
                    count = orders.Count;
                }
            }

            return Json(new { count }, JsonRequestBehavior.AllowGet);
        }
    }
}
