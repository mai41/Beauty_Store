using Beauty_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beauty_Store.Controllers
{
    public class complainController : Controller
    {
        public ActionResult acceptComplain()
        {
            return View();
        }

        public ActionResult writeComplain()
        {
            return View();
        }

        [HttpPost]
        public ActionResult writeComplain(complain comp,string complain)
        {
            using (complainEntities db = new complainEntities())
            {
                string temp = Session["userID"].ToString();
                comp.userid = int.Parse(temp);
                string temp2 = Session["id"].ToString();
                comp.saleCode = temp2;
                comp.complain1 = complain;
                db.complains.Add(comp);
                db.SaveChanges();
                return RedirectToAction("acceptComplain");
            }
        }

        public ActionResult makeComplain()
        {
            return View();
        }

        [HttpPost]
        public ActionResult makeComplain(string option, complain complain)
        {
            using (complainEntities db = new complainEntities())
            {
                string temp = Session["userID"].ToString();
                complain.userid = int.Parse(temp);
                if (option != null)
                {
                    if (option.Equals("Late delivery") || option.Equals("Delivery Man didn't Serve Me Well") || option.Equals("I have Recieved Wrong Product(s)"))
                    {
                        complain.complain1 = option;
                        using (saleEntities entities = new saleEntities())
                        {
                            var sale = entities.sales.FirstOrDefault(s => s.sale_code.Equals(complain.saleCode) && s.delivery_status.Equals(true));
                            if (sale != null)
                            {
                                db.complains.Add(complain);
                                db.SaveChanges();
                                return RedirectToAction("acceptComplain");
                            }
                        }
                    }
                    if (option.Equals("Something Else"))
                    {
                        using (saleEntities entities = new saleEntities())
                        {
                            var sale = entities.sales.FirstOrDefault(s => s.sale_code.Equals(complain.saleCode) && s.delivery_status.Equals(true));
                            if (sale != null)
                            {
                                Session["id"] = complain.saleCode.ToString();
                                return RedirectToAction("writeComplain");
                            }
                        }
                    }
                }
            }
            return View();
        }

        public ActionResult showComplains(string option)
        {
            using (complainEntities db = new complainEntities())
            {
                string option1= "Late delivery";
                string option2= "Delivery Man didn't Serve Me Well";
                string option3= "I have Recieved Wrong Product(s)";
                if (option != null)
                {
                    if (option.Equals(option1) || option.Equals(option2) || option.Equals(option3))
                    {
                        var complain = db.complains.Where(c => c.complain1.Equals(option)).ToList();
                        Session["noOfComp"] = complain.Count().ToString();
                        return View(complain);
                    }
                    if (option.Equals("Something Else"))
                    {
                        List<complain> complains = new List<complain>();
                        var complain = db.complains.ToList();
                        foreach(var x in complain)
                        {
                            if (!(x.complain1.Equals(option1) || x.complain1.Equals(option2) || x.complain1.Equals(option3)))
                            {
                                complains.Add(x);
                            }
                        }
                        Session["noOfComp"] = complains.Count().ToString();
                        return View(complains);
                    }
                }
                var entity = db.complains.ToList();
                Session["noOfComp"] = entity.Count().ToString();
                return View(entity);
            }
        }
        
    }
}