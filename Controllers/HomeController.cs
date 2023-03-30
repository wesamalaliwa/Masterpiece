using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyMasterOrange.Models;
using System.Dynamic;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace MyMasterOrange.Controllers
{
    public class HomeController : Controller
    {
        private mastermvcEntities db = new mastermvcEntities();
        public ActionResult Index()
        {
            var cate = db.categories.ToList();
            var products = db.products.Include(p => p.category).ToList();
            var data = Tuple.Create(cate, products);

            return View(data);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}