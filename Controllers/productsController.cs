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
    public class productsController : Controller
    {
        private mastermvcEntities db = new mastermvcEntities();

        // GET: products
        public async Task<ActionResult> Index()
        {
            var products = db.products.Include(p => p.category);
            return View(await products.ToListAsync());
        }





        public ActionResult Home()
        {
            var cate = db.categories.ToList();
            var products = db.products.Include(p => p.category).ToList();
            var data = Tuple.Create(cate, products);

            return View(data);
        }
        [HttpGet]
        //public ActionResult singleproduct()
        //{
        //    var products = db.products.Include(p => p.category);
        //    return View("singleproduct");
        //}
        //public  ActionResult singleproduct(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    //product product = db.products.Find(id);
        //    if (id == null)
        //    {
        //        return RedirectToAction("singleproduct");
        //    }
        //    else
        //    {
        //        var product = db.products.Find(id);
        //        var category = db.categories.Where(x => x.category_name.SingleOrDefault() == id);
        //        dynamic all = new ExpandoObject();
        //        all.m = product;
        //        all.s = category;
        //        ViewBag.messege = "get";
        //        return View(all);

        //    }
        //    //return View(product);
        //}


        public ActionResult singleproduct(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = db.products.Include(p => p.category).SingleOrDefault(p => p.product_id == id);

            if (product == null)
            {
                return HttpNotFound();
            }

            ViewBag.Quantity = product.product_quantity;

            return View(product);
        }


        //public ActionResult AddToCart(int productId, int quantity)
        //{
        //    // create a new cart item
        //    var item = new cart
        //    {
        //        product_id = productId,
        //        quantity = quantity,
        //    };

        //    // add the item to the cart
        //    var cart = GetCartFromSession(); // get the cart from session
        //    cart.Add(item); // add the item to the cart
        //    SaveCartToSession(cart); // save the updated cart to session

        //    // redirect to the cart page
        //    return RedirectToAction("Cart");
        //}
        //public ActionResult AddToCart(int productId, int quantity)
        //{
        //    // get the current user ID
        //    string userId = User.Identity.GetUserId();

        //    // check if the user already has this product in their cart
        //    var existingCartItem = db.carts.SingleOrDefault(c => c.user_id == userId && c.product_id == productId);

        //    if (existingCartItem != null)
        //    {
        //        // if the product is already in the cart, update the quantity
        //        existingCartItem.quantity += quantity;
        //    }
        //    else
        //    {
        //        // if the product is not yet in the cart, create a new cart item
        //        var cartItem = new cart
        //        {
        //            user_id = userId,
        //            product_id = productId,
        //            quantity = quantity,
        //        };

        //        db.carts.Add(cartItem);
        //    }

        //    db.SaveChanges();

        //    // redirect the user to the cart page
        //    return RedirectToAction("Cart");
        //}

        //public ActionResult AddToCart(int productId, int quantity)
        //{
        //    // Validate user input
        //    if (productId <= 0 || quantity <= 0)
        //    {
        //        return RedirectToAction("Error");
        //    }

        //    // Get the current user ID
        //    string userId = User.Identity.GetUserId();

        //    // Check if the user already has this product in their cart
        //    var existingCartItem = db.carts.FirstOrDefault(c => c.user_id == userId && c.product_id == productId);

        //    if (existingCartItem != null)
        //    {
        //        // If the user already has this product in their cart, update the quantity
        //        existingCartItem.quantity += quantity;
        //        db.Entry(existingCartItem).State = EntityState.Modified;
        //    }
        //    else
        //    {
        //        // If the user doesn't have this product in their cart, create a new cart item
        //        var cartItem = new cart
        //        {
        //            user_id = userId,
        //            product_id = productId,
        //            quantity = quantity
        //        };
        //        db.carts.Add(cartItem);
        //    }

        //    // Save changes to the database
        //    db.SaveChanges();

        //    // Redirect the user to their shopping cart
        //    return RedirectToAction("Cart");
        //}

        [HttpPost]
        public ActionResult AddToCart(FormCollection form)
        {
            // Parse form values
            int productId = Convert.ToInt32(form["product_id"]);
            int quantity = Convert.ToInt32(form["quantity"]);

            // Validate user input
            //if (productId <= 0 || quantity <= 0)
            //{
            //    return RedirectToAction("Error");
            //}

            // Get the current user ID
            string userId = User.Identity.GetUserId();

            //// Check if the user already has this product in their cart
            //var existingCartItem = db.carts.FirstOrDefault(c => c.user_id == userId && c.product_id == productId);

            //if (existingCartItem != null)
            //{
            //    // If the user already has this product in their cart, update the quantity
            //    existingCartItem.quantity += quantity;
            //    db.Entry(existingCartItem).State = EntityState.Modified;
            //}
            //else
            //{
            //    // If the user doesn't have this product in their cart, create a new cart item
            //    var cartItem = new cart
            //    {
            //        user_id = userId,
            //        product_id = productId,
            //        quantity = quantity
            //    };
            //    db.carts.Add(cartItem);
            //}

            //// Save changes to the database
            //db.SaveChanges();
            // Check if the user already has this product in their cart
            var existingCartItem = db.carts.FirstOrDefault(c => c.user_id == userId && c.product_id == productId);

            if (existingCartItem != null)
            {
                // If the user already has this product in their cart, subtract the existing quantity from the new quantity entered by the user
                quantity -= existingCartItem.quantity;
                existingCartItem.quantity += quantity;
                db.Entry(existingCartItem).State = EntityState.Modified;
            }
            else
            {
                // If the user doesn't have this product in their cart, create a new cart item
                var cartItem = new cart
                {
                    user_id = userId,
                    product_id = productId,
                    quantity = quantity
                };
                db.carts.Add(cartItem);
            }

            // Save changes to the database
            db.SaveChanges();


            // Redirect the user to their shopping cart
            return RedirectToAction("home");
        }

        public ActionResult Cart()
        {
            // Get the current user ID
            string userId = User.Identity.GetUserId();

            // Get the user's cart items
            var cartItems = db.carts.Where(c => c.user_id == userId).ToList();

            var firstCartItem = cartItems.FirstOrDefault();
            int productQuantity = 0;
            if (firstCartItem != null)
            {
                productQuantity = (int)db.products
                    .Where(p => p.product_id == firstCartItem.product_id)
                    .Select(p => p.product_quantity)
                    .FirstOrDefault();
            }

            // Pass the cart items and product quantity to the view
            ViewBag.CartItems = cartItems;
            ViewBag.ProductQuantity = productQuantity;
            // Pass the cart items to the view
            return View(cartItems);
        }

        public ActionResult ClearCart()
        {
            // Get the current user ID
            string userId = User.Identity.GetUserId();

            // Get the user's cart items
            var cartItems = db.carts.Where(c => c.user_id == userId).ToList();

            // Remove all cart items
            db.carts.RemoveRange(cartItems);

            // Save changes to the database
            db.SaveChanges();

            // Redirect the user back to the cart page
            return RedirectToAction("Cart");
        }



        public ActionResult clearproduct(int id)
        {
            // TODO: Implement the logic to clear the product with the specified ID.
            using (var db = new mastermvcEntities()) 
            {
                var product = db.products.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }

                // Perform the logic to clear the product here.
                // For example:
                db.products.Remove(product);
                db.SaveChanges();
            }

            // Redirect to the product list page.
            return RedirectToAction("Cart"); // Replace "Products" with the name of your controller.
        }



        //public ActionResult Cart1()
        //{
        //    // Get the current user ID
        //    Guid userId = new Guid(User.Identity.GetUserId());

        //    // Get the user's cart items
        //    var cartItems = db.carts.Where(c => c.Id == userId).ToList();

        //    // Calculate the total price for all products in the cart
        //    decimal totalPrice = 0;
        //    foreach (var item in cartItems)
        //    {
        //        item.Price = item.product.price; // set the price for a single item
        //        totalPrice += item.quantity * item.Price; // calculate the total price
        //    }

        //    // Pass the cart items and total price to the view
        //    ViewBag.TotalPrice = totalPrice;
        //    return View(cartItems);
        //}

        //public ActionResult singleproduct(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }

        //    var product = db.products.Include(p => p.category).SingleOrDefault(p => p.product_id == id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    var categories = db.categories.ToList();
        //    var data = Tuple.Create(categories, product);

        //    return View(data);
        //}



        //public ActionResult Checkout()
        //{
        //    // Get the current user
        //    ApplicationUser user = UserManager.FindById(User.Identity.GetUserId());

        //    // Get the user's cart items
        //    var cartItems = db.carts.Where(c => c.user_id == user.Id).ToList();

        //    // Calculate the total price of the cart
        //    decimal total = cartItems.Sum(c => c.product.product_price * c.product.product_quantity);

        //    // Create an order object
        //    Order order = new Order()
        //    {
        //        User = user,
        //        DateCreated = DateTime.Now,
        //        Total = total
        //    };

        //    // Add the order to the database
        //    db.Orders.Add(order);
        //    db.SaveChanges();

        //    // Create order items for each item in the cart
        //    foreach (var item in cartItems)
        //    {
        //        OrderItem orderItem = new OrderItem()
        //        {
        //            Order = order,
        //            Product = item.Product,
        //            Quantity = item.Quantity
        //        };
        //        db.OrderItems.Add(orderItem);
        //        db.carts.Remove(item); // Remove the cart item from the user's cart
        //    }

        //    // Save changes to the database
        //    db.SaveChanges();

        //    // Redirect the user to the order confirmation page
        //    return RedirectToAction("OrderConfirmation", new { orderId = order.Id });
        //}



        [HttpGet]
        public ActionResult Index(string searchName)
        {
            var products = from c in db.products select c ;

            if (!String.IsNullOrEmpty(searchName))
            {
                products = products.Where(c => c.product_name.Contains(searchName));
            }

            return View(products.ToList());
        }

        // GET: products/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: products/Create
        public ActionResult Create()
        {
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name");
            return View();
        }

        // POST: products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create( product product , HttpPostedFileBase img)
        {
            if (ModelState.IsValid)
            {
                if (img != null)
                {
                    string path = Server.MapPath("../Images/") + img.FileName;
                    img.SaveAs(path);
                    product.product_image1 = img.FileName;
                }
                db.products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // GET: products/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
            return View(product);
        }

        // POST: products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "product_id,product_name,product_price,product_discount,product_des,product_des2,product_image1,product_image2,product_image3,product_image4,product_image5,product_quantity,category_id")] product product)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(product).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", product.category_id);
        //    return View(product);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(product product, HttpPostedFileBase img)
        {
            var existingProduct = db.products.Find(product.product_id);
            if (existingProduct == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                existingProduct.product_name = product.product_name;
                existingProduct.product_price = product.product_price;
                existingProduct.product_discount = product.product_discount;
                existingProduct.product_des = product.product_des;
                existingProduct.product_des2 = product.product_des2;
                existingProduct.product_quantity = product.product_quantity;

                if (img != null)
                {
                    string path = Server.MapPath("~/Images/") + img.FileName;
                    img.SaveAs(path);
                    existingProduct.product_image1 = img.FileName;
                }

                db.Entry(existingProduct).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.category_id = new SelectList(db.categories, "category_id", "category_name", existingProduct.category_id);
            return View(existingProduct);
        }


        // GET: products/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            product product = await db.products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            product product = await db.products.FindAsync(id);
            db.products.Remove(product);
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
