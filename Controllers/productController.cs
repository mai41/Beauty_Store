using Beauty_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beauty_Store.Controllers
{
    public class productController : Controller
    {
        public ActionResult warningView()
        {
            return View();
        }

        public ActionResult getPerfumeList()
        {
            if(ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    var perfume = db.products.Where(p => p.product_type.Equals("perfume")).ToList();
                    return View(perfume);
                }
            }
            return View();
        }

        public ActionResult getSKList()
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    var skinCare = db.products.Where(p => p.product_type.Equals("skin care")).ToList();
                    return View(skinCare);
                }
            }
            return View();
        }

        public ActionResult getBagsList()
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    var bags = db.products.Where(p => p.product_type.Equals("bags")).ToList();
                    return View(bags);
                }
            }
            return View();
        }

        public ActionResult getAccList()
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    var Accessories = db.products.Where(p => p.product_type.Equals("accessories")).ToList();
                    return View(Accessories);
                }
            }
            return View();
        }

        public ActionResult getShoesList()
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    var shoes = db.products.Where(p => p.product_type.Equals("shoes")).ToList();
                    return View(shoes);
                }
            }
            return View();
        }

        public ActionResult showDetails(int? product_id)
        {
            using (productEntities db = new productEntities())
            {
                var entity = db.products.Where(p => p.product_id == product_id).FirstOrDefault();
                Session["prouctID"] = product_id.ToString();
                return View(entity);
            }
        }

        public ActionResult getPerfumeForAdmin(string search)
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    if (search != null)
                    {
                        return View(db.products.Where(p => p.product_type.Equals("perfume") && p.product_name.Contains(search) || search == null).ToList());
                    }
                    var perfume = db.products.Where(p => p.product_type.Equals("perfume")).ToList();
                    return View(perfume);
                }
            }
            return View();
        }

        public ActionResult getSKForAdmin(string search)
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    if (search != null)
                    {
                        return View(db.products.Where(p => p.product_type.Equals("skin care") && p.product_name.Contains(search) || search == null).ToList());
                    }
                    var skinCare = db.products.Where(p => p.product_type.Equals("skin care")).ToList();
                    return View(skinCare);
                }
            }
            return View();
        }

        public ActionResult getBagsForAdmin(string search)
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    if (search != null)
                    {
                        return View(db.products.Where(p => p.product_type.Equals("bags") && p.product_name.Contains(search) || search == null).ToList());
                    }
                    var bags = db.products.Where(p => p.product_type.Equals("bags")).ToList();
                    return View(bags);
                }
            }
            return View();
        }

        public ActionResult getAccForAdmin(string search)
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    if (search != null)
                    {
                        return View(db.products.Where(p => p.product_type.Equals("accessories") && p.product_name.Contains(search) || search == null).ToList());
                    }
                    var Accessories = db.products.Where(p => p.product_type.Equals("accessories")).ToList();
                    return View(Accessories);
                }
            }
            return View();
        }

        public ActionResult getShoesForAdmin(string search)
        {
            if (ModelState.IsValid)
            {
                using (productEntities db = new productEntities())
                {
                    if (search != null)
                    {
                        return View(db.products.Where(p => p.product_type.Equals("shoes") && p.product_name.Contains(search) || search == null).ToList());
                    }
                    var shoes = db.products.Where(p => p.product_type.Equals("shoes")).ToList();
                    return View(shoes);
                }
            }
            return View();
        }
        
        public ActionResult addProduct()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addProduct(product product)
        {
            using (productEntities db = new productEntities())
            {
                if (ModelState.IsValid)
                {
                    var entity = db.products.FirstOrDefault(p => p.product_name.Equals(product.product_name) && p.product_type.Equals(product.product_type));
                    if (product != null)
                    {
                        if (entity != null)
                        {
                            return RedirectToAction("AdminDashBoard", "admin");
                        }
                        if (product.discount_per == null || product.discount_per <= 50)
                        {
                            product.product_img = product.product_name + ".jpg";
                            string path = @"C:\Users\Win 10\Downloads\Beauty_Store\Beauty_Store\Images\";
                            string oldPath = @"C:\Users\Win 10\Downloads\";
                            string file = product.product_img;
                            string fileToMove = oldPath + file;
                            string moveTo = path + file;
                            System.IO.File.Copy(fileToMove, moveTo, true);
                            db.products.Add(product);
                            db.SaveChanges();
                            return RedirectToAction("AdminDashBoard", "admin");
                        }
                    }
                }
            }
            return View(product);
        }

        public ActionResult editProduct(int? product_id)
        {
            bool check = true;
            using (saleEntities entities = new saleEntities())
            {
                var sales = entities.sales.Where(s => s.productid == product_id).ToList();
                if (sales != null)
                {
                    foreach (var x in sales)
                    {
                        if (x.delivery_status == false)
                        {
                            check = false;
                            break;
                        }
                    }
                }
            }
            using (productEntities db = new productEntities())
            {
                var entity = db.products.Where(u => u.product_id == product_id).FirstOrDefault();
                Session["productID"] = product_id.ToString();
                if (check == false)
                {
                    return RedirectToAction("warningView");
                }
                return View(entity);
            }
        }

        [HttpPost]
        public ActionResult editProduct(product product)
        {
            using (productEntities db = new productEntities())
            {
                if (ModelState.IsValid)
                {
                    string temp = Session["productID"].ToString();
                    product.product_id = int.Parse(temp);
                    var entity = db.products.Where(u => u.product_id == product.product_id).FirstOrDefault();
                    string img = entity.product_img;
                    if (entity != null)
                    {
                        if (product.discount_per == null || product.discount_per <= 50)
                        {
                            entity.product_name = product.product_name;
                            entity.product_type = product.product_type;
                            entity.price = product.price;
                            entity.year = product.year;
                            entity.discount_per = product.discount_per;
                            entity.shipping_exp = product.shipping_exp;
                            entity.product_img = product.product_name + ".jpg";
                            if (!(img.Equals(entity.product_img)))
                            {
                                string path = @"C:\Users\Win 10\Downloads\Beauty_Store\Beauty_Store\Images\";
                                string oldPath = @"C:\Users\Win 10\Downloads\";
                                string file = entity.product_img;
                                string fileToMove = oldPath + file;
                                string moveTo = path + file;
                                System.IO.File.Copy(fileToMove, moveTo, true);
                            }
                            db.SaveChanges();
                            return RedirectToAction("AdminDashBoard", "admin");
                        }
                    }
                }
            }
            return View(product);
        }

        public ActionResult deleteProduct(product product)
        {
            bool check = true;
            using (saleEntities entities = new saleEntities())
            {
                var sales = entities.sales.Where(s => s.productid == product.product_id).ToList();
                if (sales != null)
                {
                    foreach (var x in sales)
                    {
                        if (x.delivery_status == false)
                        {
                            check = false;
                            break;
                        }
                    }
                }
            }
            using (productEntities db = new productEntities())
            {
                var entity = db.products.FirstOrDefault(c => c.product_id == product.product_id);
                if (check == false)
                {
                    return RedirectToAction("warningView");
                }
                if (entity != null)
                {
                    db.products.Remove(entity);
                    db.SaveChanges();
                    return RedirectToAction("AdminDashBoard", "admin");
                }
            }
            return RedirectToAction("AdminDashBoard", "admin");
        }

    }
}