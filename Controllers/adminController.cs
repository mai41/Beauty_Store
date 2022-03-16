using Beauty_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beauty_Store.Controllers
{
    public class adminController : Controller
    {
        public ActionResult addAdminSuccessfully()
        {
            return View();
        }

        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(adminEdit user)
        {
            using (adminEntities db = new adminEntities())
            {
                var obj = db.Admins.FirstOrDefault(u => u.admin_name.Equals(user.admin_name) && u.password.Equals(user.password));
                if (obj != null)
                {
                    Session["adminID"] = obj.admin_id.ToString();
                    Session["adminName"] = obj.admin_name.ToString();
                    Session["adminPassword"] = obj.password.ToString();
                    Session["adminEmail"] = obj.email.ToString();
                    Session["adminPhoneNo"] = obj.phoneNo.ToString();
                    return RedirectToAction("AdminDashBoard");
                }
            }
            return View(user);
        }

        public ActionResult addAdmin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addAdmin(Admin user)
        {
            using (adminEntities db = new adminEntities())
            {
                if (ModelState.IsValid)
                {
                    var entity = db.Admins.FirstOrDefault(a => a.admin_name.Equals(user.admin_name) && a.password.Equals(user.password));
                    if (entity == null && user != null)
                    {
                        db.Admins.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("addAdminSuccessfully");
                    }
                }
            }
            return View(user);
        }

        public ActionResult lockProfile()
        {
            using (adminEntities db = new adminEntities())
            {
                string temp = Session["adminID"].ToString();
                int id = int.Parse(temp);
                var entity = db.Admins.FirstOrDefault(a => a.admin_id == id);
                if (entity != null)
                {
                    db.Admins.Remove(entity);
                    db.SaveChanges();
                    return RedirectToAction("login");
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Edit(int? user_id)
        {
            using (adminEntities db = new adminEntities())
            {
                string temp = Session["adminID"].ToString();
                user_id = int.Parse(temp);
                var entity = db.Admins.Where(u => u.admin_id == user_id).FirstOrDefault();
                return View(entity);
            }
        }

        [HttpPost]
        public ActionResult Edit(Admin user)
        {
            using (adminEntities db = new adminEntities())
            {
                string temp = Session["adminID"].ToString();
                user.admin_id = int.Parse(temp);
                var entity = db.Admins.Where(u => u.admin_id == user.admin_id).FirstOrDefault();
                if (entity != null)
                {
                    entity.admin_name = user.admin_name;
                    entity.email = user.email;
                    entity.password = user.password;
                    entity.phoneNo = user.phoneNo;
                    db.SaveChanges();
                    Session["adminName"] = entity.admin_name.ToString();
                    Session["adminPassword"] = entity.password.ToString();
                    Session["adminEmail"] = entity.email.ToString();
                    Session["adminPhoneNo"] = entity.phoneNo.ToString();
                    return RedirectToAction("AdminDashBoard");
                }

                return View(user);
            }
        }

        public ActionResult AdminDashBoard()
        {
            if (Session["adminName"] != null && Session["adminPassword"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
        }

    }
}