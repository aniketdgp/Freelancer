using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Web.Security;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FreeLancer.Models;
using System.IO;



namespace FreeLancer.Controllers
{
    public class freelancerController : Controller
    {
        // GET: freelancer
        private UsersEntities2 db = new UsersEntities2();
        public int i = 1;

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult freelancerhome()
        {
            return View();
        }

        public ActionResult Loggeduser()
        {
         
            return View(db.jobs.ToList());
        }


        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(logindata l)
        {
            var adminid = "admin@aniket";
            var adminpas = "admin";


            if (ModelState.IsValid)
            {
                var obj = db.signups.Where(a => a.Email.Equals(l.Email) && a.Password.Equals(l.password)).FirstOrDefault();
                if (obj != null)
                {
                    Session["Email"] = obj.Email.ToString();
                    Session["Password"] = obj.Password.ToString();

                    if (Session["Email"].Equals(adminid)&& Session["password"].Equals(adminpas))
                    {
                        return RedirectToAction("freelancerhome");
                    }

                    else
                    {
                        return RedirectToAction("Loggeduser");
                    }
 
                    



                }
                else
                {
                    return RedirectToAction("login");
                }

            }

            return View();
        }


        public ActionResult signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signup1(signup s)
        {

            s.Id = Convert.ToInt32(s.Contact.Substring(2, 5));
            try
            {
               
                if (ModelState.IsValid)
                {

                    db.signups.Add(s);
                    db.SaveChanges();
                    ViewBag.m = "Submitted";

                }
                else { ViewBag.m = "not available"; }
            }
            catch(Exception e)
            {
                ViewBag.m = "already Registered Mobile No";
            }
            return View("signup");
        }
        public ActionResult cv()
        {
            return View();
        }


        [HttpPost]
        public ActionResult cv(cv c)
        {

            if (ModelState.IsValid)
            {
                

                db.cvs.Add(c);
                db.SaveChanges();
                ViewBag.m = "Submitted";

            }
            return View();
        }

        public ActionResult Show()
        {
            string t="",a="",b="";
            int i = 0;
            string s1 = Session["Email"].ToString();
            IQueryable<cv> q = from s in db.cvs
                               where s.Email == s1
                               select s;
            foreach (var cust in q)
            {
                t = t +"NAME:"+ cust.Name;
                a = a + "Email:" + cust.Email;
                b = b + "Age:" + cust.Age;
            }
            ViewBag.t = t;
            ViewBag.a = a;
            ViewBag.b = b;




            return View();
        }



        public ActionResult logout()
        {
            Session.Remove("Email");
            return RedirectToAction("Index");
        }
        public ActionResult Admin()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Admin(job j)
        {
            HttpPostedFileBase file = Request.Files["ImageUpload"];

            string pic = System.IO.Path.GetFileName(file.FileName);
            string path =System.IO.Path.Combine(Server.MapPath("~/Content/img"),pic);
            file.SaveAs(path);
            ViewBag.m = pic;
            j.photo = pic;
            if (ModelState.IsValid)
            {
              
                    db.jobs.Add(j);
                    db.SaveChanges();
                    ViewBag.m = "Submitted";
                }
            


            return View();
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult Jobdata()
        {
            return View(db.jobs.ToList());
          
        }


    }
}