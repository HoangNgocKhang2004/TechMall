using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechMall.Context;
using TechMall.Models;

namespace TechMall.Controllers
{
    public class CategoryController : Controller
    {
        WebAspDbEntities objWebAspDbEntities = new WebAspDbEntities();
        // GET: Category
        public ActionResult Index()
        {
            HomeModel objHomeModel = new HomeModel();
            objHomeModel.ListCategory = objWebAspDbEntities.Categories.ToList();
            return View(objHomeModel);
        }
    }
}