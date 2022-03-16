using Beauty_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beauty_Store.Controllers
{
    public class saleController : Controller
    {
        public ActionResult getFinalPage()
        {
            return View();
        }

        public ActionResult makeSale()
        {
            return View();
        }

        [HttpPost]
        public ActionResult makeSale(sale sale)
        {
            if (ModelState.IsValid)
            {
                using (saleEntities db = new saleEntities())
                {
                    string temp = Session["userID"].ToString();
                    sale.userid = int.Parse(temp);
                    string proID = Session["prouctID"].ToString();
                    sale.productid = int.Parse(proID);
                    var entity = db.sales.FirstOrDefault(s => s.userid == sale.userid && s.productid == sale.productid && s.delivery_status.Equals(false));
                    if (entity != null)
                    {
                        entity.no_of_products = entity.no_of_products + sale.no_of_products;
                        Session["salecode"] = entity.sale_code.ToString();
                        db.SaveChanges();
                        return RedirectToAction("getFinalPage");
                    }
                    var saleList = db.sales.ToList();
                    sale.Date = DateTime.Today;
                    sale.sale_code = "Beauty" + (saleList.Last().sale_id+1);
                    Session["salecode"] = sale.sale_code.ToString();
                    sale.delivery_status = false;
                    if (sale != null)
                    {
                        db.sales.Add(sale);
                        db.SaveChanges();
                        return RedirectToAction("getFinalPage");
                    }
                }
            }
            return View(sale);
        }

        public ActionResult editSale(int? product_id)
        {
            using (saleEntities db = new saleEntities())
            {
                string temp = Session["userID"].ToString();
                int user_id = int.Parse(temp);
                var entity = db.sales.FirstOrDefault(s => s.userid == user_id && s.productid == product_id);
                Session["saleid"] = entity.sale_id.ToString();
                return View(entity);
            }
        }

        [HttpPost]
        public ActionResult editSale(sale sale)
        {
            if (ModelState.IsValid)
            {
                using (saleEntities db = new saleEntities())
                {
                    string temp = Session["saleid"].ToString();
                    int sale_id = int.Parse(temp);
                    var entity = db.sales.FirstOrDefault(s => s.sale_id == sale_id);
                    if (sale != null && entity != null)
                    {
                        entity.no_of_products = sale.no_of_products;
                        db.SaveChanges();
                        return RedirectToAction("showSales");
                    }
                }
            }
            return View(sale);
        }

        public ActionResult showSales(string option, string search)
        {
            List<product> proList = new List<product>();
            List<product> products = new List<product>();
            using (saleEntities db = new saleEntities())
            {
                string temp = Session["userID"].ToString();
                int userid = int.Parse(temp);
                var salesList = db.sales.Where(s => s.userid == userid && s.delivery_status.Equals(false)).ToList();
                foreach (var x in salesList)
                {
                    using (productEntities entities = new productEntities())
                    {
                        if (option != null)
                        {
                            if (option.Equals("Product Name"))
                            {
                                products.Clear();
                                var entity = entities.products.Where(c => c.product_name.Contains(search) || search == null).ToList();
                                foreach(var pro in entity)
                                {
                                    var s = db.sales.FirstOrDefault(p => p.userid == userid && p.productid == pro.product_id&& p.delivery_status.Equals(false));
                                    if (s != null)
                                    {
                                        products.Add(pro);
                                    }
                                }
                                return View(products);
                            }
                            if (option.Equals("Product Type"))
                            {
                                products.Clear();
                                var entity = entities.products.Where(c => c.product_type.Contains(search) || search == null).ToList();
                                foreach (var pro in entity)
                                {
                                    var s = db.sales.FirstOrDefault(p => p.userid == userid && p.productid == pro.product_id && p.delivery_status.Equals(false));
                                    if (s != null)
                                    {
                                        products.Add(pro);
                                    }
                                }
                                return View(products);
                            }
                            if (option.Equals("Price"))
                            {
                                int ss = int.Parse(search);
                                products.Clear();
                                var entity = entities.products.Where(c => c.price == ss || search == null).ToList();
                                foreach (var pro in entity)
                                {
                                    var s = db.sales.FirstOrDefault(p => p.userid == userid && p.productid == pro.product_id && p.delivery_status.Equals(false));
                                    if (s != null)
                                    {
                                        products.Add(pro);
                                    }
                                }
                                return View(products);
                            }
                        }
                        var product = entities.products.FirstOrDefault(p => p.product_id == x.productid);
                        proList.Add(product);
                    }
                }
            }
            return View(proList);
        }

        public ActionResult cancelSale(int? product_id)
        {
            using (saleEntities db = new saleEntities())
            {
                string temp = Session["userID"].ToString();
                int user_id = int.Parse(temp);
                var entity = db.sales.FirstOrDefault(s => s.userid == user_id && s.productid == product_id);
                if (entity != null)
                {
                    db.sales.Remove(entity);
                    db.SaveChanges();
                }
                return RedirectToAction("showSales");
            }
        }

        public ActionResult changeDeliveryStatus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult changeDeliveryStatus(sale sale)
        {
            using (saleEntities db = new saleEntities())
            {
                var entity = db.sales.FirstOrDefault(s => s.sale_code.Equals(sale.sale_code) && s.delivery_status.Equals(false));
                if (entity != null)
                {
                    entity.delivery_status = true;
                    db.SaveChanges();
                    return RedirectToAction("AdminDashBoard", "admin");
                }
                if (entity == null)
                {
                    return RedirectToAction("AdminDashBoard", "admin");
                }
            }
            return View(sale);
        }

        public ActionResult getProductSales(int? product_id)
        {
            using (saleEntities db = new saleEntities())
            {
                var sales = db.sales.Where(s => s.productid == product_id).ToList();
                return View(sales);
            }
        }

        public ActionResult getCode(int? product_id)
        {
            using (saleEntities db = new saleEntities())
            {
                string temp = Session["userID"].ToString();
                int userid = int.Parse(temp);
                var sale = db.sales.FirstOrDefault(s => s.productid == product_id && s.userid == userid && s.delivery_status == false);
                Session["salecode"] = sale.sale_code.ToString();
                return RedirectToAction("getFinalPage");
            }
        }
    }
}