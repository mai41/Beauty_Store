using Beauty_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Beauty_Store.Controllers
{
    public class userController : Controller
    {
        public ActionResult getHomePage()
        {
            return View();
        }
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(userEdit user)
        {
            using (userEntities db = new userEntities())
            {
                var obj = db.users.FirstOrDefault(u => u.user_name.Equals(user.user_name) && u.password.Equals(user.password));
                if (obj != null)
                {
                    Session["userID"] = obj.user_id.ToString();
                    Session["userName"] = obj.user_name.ToString();
                    Session["password"] = obj.password.ToString();
                    Session["address"] = obj.address.ToString();
                    Session["city"] = obj.city.ToString();
                    Session["email"] = obj.email.ToString();
                    Session["phoneNo"] = obj.phoneNo.ToString();
                    return RedirectToAction("UserDashBoard");
                }
            }
            return View(user);
        }

        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(user user)
        {
            using (userEntities db = new userEntities())
            {
                var entity = db.users.FirstOrDefault(u => u.user_name.Equals(user.user_name) && u.password.Equals(user.password));
                if (entity == null && user != null)
                {
                    db.users.Add(user);
                    db.SaveChanges();
                    Session["userID"] = user.user_id.ToString();
                    Session["userName"] = user.user_name.ToString();
                    Session["password"] = user.password.ToString();
                    Session["address"] = user.address.ToString();
                    Session["city"] = user.city.ToString();
                    Session["email"] = user.email.ToString();
                    Session["phoneNo"] = user.phoneNo.ToString();
                    return RedirectToAction("UserDashBoard2");
                }
            }
            return View(user);
        }
        
        [HttpGet]
        public ActionResult Edit(int? user_id)
        {
            using (userEntities db = new userEntities())
            {
                string temp = Session["userID"].ToString();
                user_id = int.Parse(temp);
                var entity = db.users.Where(u => u.user_id == user_id).FirstOrDefault();
                return View(entity);
            }
        }

        [HttpPost]
        public ActionResult Edit(user user)
        {
            using (userEntities db = new userEntities())
            {
                string temp = Session["userID"].ToString();
                user.user_id = int.Parse(temp);
                var entity = db.users.Where(u => u.user_id == user.user_id).FirstOrDefault();
                if (entity != null)
                {
                    entity.user_name = user.user_name;
                    entity.email = user.email;
                    entity.address = user.address;
                    entity.city = user.city;
                    entity.password = user.password;
                    entity.phoneNo = user.phoneNo;
                    db.SaveChanges();
                    Session["userName"] = entity.user_name.ToString();
                    Session["password"] = entity.password.ToString();
                    Session["address"] = entity.address.ToString();
                    Session["city"] = entity.city.ToString();
                    Session["email"] = entity.email.ToString();
                    Session["phoneNo"] = entity.phoneNo.ToString();
                    return RedirectToAction("UserDashBoard");
                }

                return View(user);
            }
        }

        public ActionResult getCustomerInfo(int? userid)
        {
            using (userEntities db = new userEntities())
            {
                var userList = db.users.Where(u => u.user_id == userid).ToList();
                return View(userList);
            }
        }

        public ActionResult UserDashBoard()
        {
            if (Session["userName"] != null && Session["password"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("login");
            }
        }

        public ActionResult UserDashBoard2()
        {
            if (Session["userName"] != null && Session["password"] != null && Session["address"] != null
                && Session["email"] != null && Session["phoneNo"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("signup");
            }
        }
    }
}