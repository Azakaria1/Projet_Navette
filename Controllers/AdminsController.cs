using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projet_Navette.Models;

namespace Projet_Navette.Controllers
{
    public class AdminsController : Controller
    {
        public static bool IsValidEmailAddress(string emailaddress)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailaddress);
                return addr.Address == emailaddress;
            }
            catch
            {
                return false;
            }
        }

        private Gestion_NavettesEntities1 db = new Gestion_NavettesEntities1();

        public ActionResult Index()
        {
            if (!((Admin)Session["admin"]).email.Equals("admin@gmail.com"))
                return RedirectToAction("connexion");
                return View(db.Admin.ToList());
        }

        // GET: Admins/Details/5

        [Route("Admin/connexion")]
        public ActionResult connexion()
        {
            return View();
        }

        [Route("Admin/connexion")]
        [HttpPost]
        public ActionResult connexion(Admin admin)
        {
            var existe = db.Admin.Where(a => a.email == admin.email && a.password == admin.password).FirstOrDefault();
            if (existe != null)
            {
                Session["admin"] = existe;
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }

        // GET: Admins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Admin admin)
        {
            if (ModelState.IsValid && IsValidEmailAddress(admin.email) )
            {
                admin.date_ajout = DateTime.Now;
                db.Admin.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Villes/Edit/5
        public ActionResult Edit(int id)
        {
            Admin admin = db.Admin.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            if (!((Admin)Session["admin"]).email.Equals("admin@gmail.com"))
                return RedirectToAction("connexion", "Admins");
            else
                return View(admin);
        }

        // POST: Admins/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Admin admin)
        {

            if (!((Admin)Session["admin"]).email.Equals("admin@gmail.com"))
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid && IsValidEmailAddress(admin.email))
                {
                    db.Entry(admin).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(admin);
            }
        }

        public ActionResult Delete(int id)
        {
            if (!((Admin)Session["admin"]).email.Equals("admin@gmail.com"))
                return RedirectToAction("connexion", "Admins");

            Admin admin = db.Admin.Find(id);
            db.Admin.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult deconnexion()
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            Session["admin"] = null;
            Session.Contents.RemoveAll();
            return RedirectToAction("connexion", "Admins");
        }
    }
}
