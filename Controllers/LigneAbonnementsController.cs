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
    public class LigneAbonnementsController : Controller
    {
        private Gestion_NavettesEntities1 db = new Gestion_NavettesEntities1();

        // GET: LigneAbonnements
        public ActionResult Index()
        {
            var ligneAbonnement = db.LigneAbonnement.Include(l => l.Client).Include(l => l.Offre);
            return View(ligneAbonnement.ToList());
        }

        // GET: LigneAbonnements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LigneAbonnement ligneAbonnement = db.LigneAbonnement.Find(id);
            if (ligneAbonnement == null)
            {
                return HttpNotFound();
            }
            return View(ligneAbonnement);
        }

        // GET: LigneAbonnements/Create
        public ActionResult Create()
        {
            ViewBag.id_Client = new SelectList(db.Client, "id_Client", "email");
            ViewBag.id_Offre = new SelectList(db.Offre, "id_Offre", "description");
            return View();
        }

        // POST: LigneAbonnements/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_LigneAbonnement,id_Client,id_Offre")] LigneAbonnement ligneAbonnement)
        {
            if (ModelState.IsValid)
            {
                db.LigneAbonnement.Add(ligneAbonnement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_Client = new SelectList(db.Client, "id_Client", "email", ligneAbonnement.id_Client);
            ViewBag.id_Offre = new SelectList(db.Offre, "id_Offre", "description", ligneAbonnement.id_Offre);
            return View(ligneAbonnement);
        }

        // GET: LigneAbonnements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LigneAbonnement ligneAbonnement = db.LigneAbonnement.Find(id);
            if (ligneAbonnement == null)
            {
                return HttpNotFound();
            }
            var nomcomplet = ligneAbonnement.Client.nom + " " + ligneAbonnement.Client.prenom;

            ViewBag.id_Client = new SelectList(db.Client, "id_Client", nomcomplet, ligneAbonnement.id_Client);
            ViewBag.id_Offre = new SelectList(db.Offre, "id_Offre", "description", ligneAbonnement.id_Offre);
            return View(ligneAbonnement);
        }

        // POST: LigneAbonnements/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_LigneAbonnement,id_Client,id_Offre")] LigneAbonnement ligneAbonnement)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ligneAbonnement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var nomcomplet = ligneAbonnement.Client.nom + " " + ligneAbonnement.Client.prenom;

            ViewBag.id_Client = new SelectList(db.Client, "id_Client", nomcomplet, ligneAbonnement.id_Client);
            ViewBag.id_Offre = new SelectList(db.Offre, "id_Offre", "description", ligneAbonnement.id_Offre);
            return View(ligneAbonnement);
        }
        public ActionResult Delete(int id)
        {
          /*  if (Session["societe"] != null)
                return RedirectToAction("connexion", "Societes");
*/
           /* else if (Session["client"] == null || Session["admin"] == null)
            {*/
                LigneAbonnement ligneAbonnement = db.LigneAbonnement.Find(id);
                db.LigneAbonnement.Remove(ligneAbonnement);
                db.SaveChanges();
                return RedirectToAction("Index");
           // return View();
        }
    }
}
