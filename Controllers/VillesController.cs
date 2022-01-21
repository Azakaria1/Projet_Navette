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
    public class VillesController : Controller
    {
        private Gestion_NavettesEntities1 db = new Gestion_NavettesEntities1();

        // GET: Villes
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");

            return View(db.Ville.ToList());
        }

        // GET: Villes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ville ville = db.Ville.Find(id);
            if (ville == null)
            {
                return HttpNotFound();
            }
            return View(ville);
        }

        // GET: Villes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Villes/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Ville,nom,pays")] Ville ville)
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid)
                {
                    db.Ville.Add(ville);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(ville);
            }
        }

        // GET: Villes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ville ville = db.Ville.Find(id);
            if (ville == null)
            {
                return HttpNotFound();
            }
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
                return View(ville);
        }

        // POST: Villes/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Ville,nom,pays")] Ville ville)
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid)
                {
                    db.Entry(ville).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(ville);
            }
        }

        // GET: Villes/Delete/5
        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                Ville ville = db.Ville.Find(id);
                db.Ville.Remove(ville);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}
