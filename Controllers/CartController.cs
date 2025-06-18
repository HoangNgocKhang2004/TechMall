using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using TechMall.Models;
using TechMall.Models.ViewModels; // ViewModel chứa ProductVM, OrderVM

namespace TechMall.Controllers
{
    public class CartController : Controller
    {
        private readonly string apiBaseUrl = "http://localhost:8080/api/";

        // GET: Cart
        public ActionResult Index()
        {
            return View((List<CartModel>)Session["cart"]);
        }

        public async Task<ActionResult> AddToCart(int id, int quantity)
        {
            ProductVM product = null;

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync(apiBaseUrl + $"products/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    product = JsonConvert.DeserializeObject<ProductVM>(json);
                }
                else
                {
                    return Json(new { Message = "Không tìm thấy sản phẩm" }, JsonRequestBehavior.AllowGet);
                }
            }

            if (Session["cart"] == null)
            {
                var cart = new List<CartModel> { new CartModel { Product = product, Quantity = quantity } };
                Session["cart"] = cart;
                Session["count"] = 1;
            }
            else
            {
                var cart = (List<CartModel>)Session["cart"];
                int index = cart.FindIndex(p => p.Product.Id == id);
                if (index != -1)
                {
                    cart[index].Quantity += quantity;
                }
                else
                {
                    cart.Add(new CartModel { Product = product, Quantity = quantity });
                    Session["count"] = Convert.ToInt32(Session["count"]) + 1;
                }
                Session["cart"] = cart;
            }

            return Json(new { Message = "Thành công" }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Remove(int id)
        {
            var cart = (List<CartModel>)Session["cart"];
            if (cart != null)
            {
                var item = cart.FirstOrDefault(x => x.Product.Id == id);
                if (item != null)
                {
                    cart.Remove(item);
                    Session["cart"] = cart;
                    Session["count"] = cart.Count;
                }
            }
            return Json(new { Message = "Xóa sản phẩm thành công", Count = Session["count"] }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCartSummary()
        {
            var cart = (List<CartModel>)Session["cart"];
            if (cart == null || cart.Count == 0)
            {
                return Json(new { TotalPrice = 0, Discount = 0, FinalPrice = 0 }, JsonRequestBehavior.AllowGet);
            }

            double totalPrice = cart.Sum(x => x.Product.Price * x.Quantity);
            double discount = 0.0;
            double finalPrice = totalPrice - discount;

            return Json(new
            {
                TotalPrice = totalPrice,
                Discount = discount,
                FinalPrice = finalPrice
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ClearCart()
        {
            Session["cart"] = null;
            Session["count"] = 0;
            return Json(new { Message = "Giỏ hàng đã được xóa" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreatOrder(string currentOrderDescription)
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "Home");
            }

            var listCart = (List<CartModel>)Session["cart"];
            if (listCart == null || listCart.Count == 0)
            {
                return Json(new { Message = "Giỏ hàng trống" }, JsonRequestBehavior.AllowGet);
            }

            if (string.IsNullOrEmpty(currentOrderDescription))
            {
                return Json(new { Message = "Mô tả đơn hàng không hợp lệ" }, JsonRequestBehavior.AllowGet);
            }

            int userId = int.Parse(Session["idUser"].ToString());

            var order = new
            {
                Name = currentOrderDescription,
                UserId = userId,
                CreatedAt = DateTime.Now,
                Status = 1
            };

            int orderId;
            using (HttpClient client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(order), Encoding.UTF8, "application/json");
                var response = await client.PostAsync(apiBaseUrl + "orders", content);
                if (!response.IsSuccessStatusCode)
                {
                    return Json(new { Message = "Tạo đơn hàng thất bại" }, JsonRequestBehavior.AllowGet);
                }

                var orderJson = await response.Content.ReadAsStringAsync();
                var orderCreated = JsonConvert.DeserializeObject<OrderVM>(orderJson);
                orderId = orderCreated.Id;

                foreach (var item in listCart)
                {
                    var detail = new
                    {
                        OrderId = orderId,
                        ProductId = item.Product.Id,
                        UserId = userId,
                        Quantity = item.Quantity,
                        Price = item.Product.Price,
                        TotalPrice = item.Product.Price * item.Quantity,
                        CreatedAt = DateTime.Now
                    };

                    var detailContent = new StringContent(JsonConvert.SerializeObject(detail), Encoding.UTF8, "application/json");
                    await client.PostAsync(apiBaseUrl + "orderdetails", detailContent);
                }
            }

            Session["cart"] = null;
            Session["count"] = 0;

            return Json(new { Message = "Đơn hàng tạo thành công", OrderId = orderId }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateQuantity(int id, int quantity)
        {
            var cart = Session["cart"] as List<CartModel>;
            var item = cart?.FirstOrDefault(x => x.Product.Id == id);

            if (item != null)
            {
                item.Quantity = quantity;
                Session["cart"] = cart;
                return Json(new { Success = true });
            }

            return Json(new { Success = false, Message = "Sản phẩm không tồn tại trong giỏ hàng." });
        }
    }
}
