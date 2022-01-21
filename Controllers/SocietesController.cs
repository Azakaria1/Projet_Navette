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
    public class SocietesController : Controller
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

     
        // GET: Societes
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");

            return View(db.Societe.ToList());
        }

        [Route("Societe/inscription")]

        public ActionResult inscription()
        {
            return View();
        }
        [Route("Societe/inscription")]
        [HttpPost]
        public ActionResult inscription(Societe societe)
        {
            if (ModelState.IsValid)
            {
                if (IsValidEmailAddress(societe.email))
                {
                    db.Societe.Add(societe);
                    db.SaveChanges();
                    return RedirectToAction("connexion");
                }

            }
            return View(societe);
        }

        [Route("Societe/connexion")]
        public ActionResult connexion()
        {
            return View();
        }

        [Route("Societe/connexion")]
        [HttpPost]
        public ActionResult connexion(Societe societe)
        {
            var existe = db.Societe.Where(a => a.email == societe.email && a.password == societe.password).FirstOrDefault();
            if (existe != null)
            {
                Session["societe"] = existe;
                return RedirectToAction("Index", "Home");
            }
            else return View();

        }
        // GET: Societes/Details/5
        public ActionResult Details(int id)
        {
            Societe societe = db.Societe.Find(id);

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            return View(societe);
        }

        // GET: Societes/Create
        public ActionResult Create()
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            return View();
        }

        // POST: Societes/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Societe,num_patente,nom,email,password,role,telephone,addresse")] Societe societe)
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid && IsValidEmailAddress(societe.email) )
                {
                    db.Societe.Add(societe);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(societe);
        }

        // GET: Societes/Edit/5
        public ActionResult Edit(int id)
        {
            Societe societe = db.Societe.Find(id);
             if (Session["admin"] != null)
                return View(societe);

            return RedirectToAction("Index", "Home");
        }

        // POST: Societes/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Societe societe)
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid && IsValidEmailAddress(societe.email))
                {
                    db.Entry(societe).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(societe);
            }
        }

        // GET: Societes/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            Societe societe = db.Societe.Find(id);
            db.Societe.Remove(societe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult deconnexion()
        {

            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            Session["societe"] = null;
            Session.Contents.RemoveAll();
            return RedirectToAction("connexion", "Societes");
        }
    }
}
