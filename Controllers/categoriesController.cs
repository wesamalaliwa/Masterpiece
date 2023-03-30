using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyMasterOrange.Models;


namespace MyMasterOrange.Controllers
{
    public class categoriesController : Controller
    {
        private mastermvcEntities db = new mastermvcEntities();

        // GET: categories
        public async Task<ActionResult> Index()
        {
            return View(await db.categories.ToListAsync());
        }
        [HttpGet]
        public ActionResult Index(string searchName)
        {
            var categories = from c in db.categories select c;

            if (!String.IsNullOrEmpty(searchName))
            {
                categories = categories.Where(c => c.category_name.Contains(searchName));
            }

            return View(categories.ToList());
        }

        // GET: categories/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = await db.categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }



        // GET: categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(category category , HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    string path = Server.MapPath("../Images/") + img.FileName;
                    img.SaveAs(path);
                    category.category_image1 = img.FileName;
                }
                db.categories.Add(category);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: categories/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = await db.categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "category_id,category_name,Category_des,Category_des2,category_image1,category_image2,category_image3")] category category, HttpPostedFileBase img)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (img != null)
        //        {
        //            string path = Server.MapPath("../Images/") + img.FileName;
        //            img.SaveAs(path);
        //            category.category_image1 = img.FileName;
        //        }

        //        db.Entry(category).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    return View(category);
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(category category, HttpPostedFileBase img)
        {
            var existingCategory = db.categories.Find(category.category_id);
            if (existingCategory == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                existingCategory.category_name = Request["category_name"];
                existingCategory.Category_des = Request["Category_des"];
                existingCategory.Category_des2 = Request["Category_des2"];

                if (img != null)
                {
                    string path = Server.MapPath("~/Images/") + img.FileName;
                    img.SaveAs(path);
                    existingCategory.category_image1 = img.FileName;
                }

                db.Entry(existingCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(existingCategory);
        }


        // GET: categories/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            category category = await db.categories.FindAsync(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // POST: categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            category category = await db.categories.FindAsync(id);
            db.categories.Remove(category);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
