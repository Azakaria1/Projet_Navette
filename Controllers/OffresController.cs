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
    public class OffresController : Controller
    {
        private Gestion_NavettesEntities1 db = new Gestion_NavettesEntities1();

        // GET: Offres
        public ActionResult Index()
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
           
                int id = ((Societe)Session["societe"]).id_Societe;
                var offre = db.Offre.Include(a => a.Societe).Where(r => r.id_Societe == id);
                return View(offre);
        }
        public ActionResult AdminIndex()
        {
            if (Session["societe"] != null)
                return RedirectToAction("Index", "Home");
            var offre = db.Offre.Include(o => o.Autocar).Include(o => o.Ville).Include(o => o.Ville1);
            return View(offre.ToList());
        }

        // GET: Offres/Details/5
        public ActionResult Details(int id)
        {
            if (Session["admin"] != null)
                return RedirectToAction("connexion", "Clients");
            Offre offre = db.Offre.Find(id);
            return View(offre);
        }

        // GET: Offres/Create
        public ActionResult Create()
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            ViewBag.id_Autocar = new SelectList(db.Autocar, "id_Autocar", "description");
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom");
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom");
            return View();
        }

        // POST: Offres/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Offre,heure_Depart,heure_Arrivee,description,prix,date_Depart,date_Arrivee,nb_Abonnes_Voulus,nb_Abonnes_Atteints,id_Societe,id_VilleArrivee,id_VilleDepart,id_Autocar")] Offre offre)
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            if (ModelState.IsValid
                         && offre.date_Arrivee >= offre.date_Depart
                         && offre.heure_Arrivee != offre.heure_Depart
                         && offre.heure_Arrivee > offre.heure_Depart
                         && offre.date_Offre < offre.date_Depart
                         && offre.id_VilleArrivee != offre.id_VilleDepart
                         )
            {
                offre.id_Societe = ((Societe)Session["societe"]).id_Societe;
                offre.nb_Abonnes_Atteints = 0;
                offre.date_Offre = DateTime.Now ;
                db.Offre.Add(offre);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_Autocar = new SelectList(db.Autocar, "id_Autocar", "description", offre.id_Autocar);
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom", offre.id_VilleArrivee);
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom", offre.id_VilleDepart);
            return View(offre);
        }

        // GET: Offres/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            
            Offre offre = db.Offre.Find(id);
            ViewBag.id_Autocar = new SelectList(db.Autocar, "id_Autocar", "description", offre.id_Autocar);
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom", offre.id_VilleArrivee);
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom", offre.id_VilleDepart);
            return View(offre);
        }

        // POST: Offres/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Offre offre)
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            if (ModelState.IsValid
                         && offre.date_Depart != offre.date_Arrivee
                         && offre.heure_Arrivee != offre.heure_Depart
                         && offre.heure_Arrivee > offre.heure_Depart
                         && offre.date_Depart < offre.date_Arrivee 
                         && offre.id_VilleArrivee != offre.id_VilleDepart

                         )
            {
                offre.id_Societe = ((Societe)Session["societe"]).id_Societe;
                db.Entry(offre).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_Autocar = new SelectList(db.Autocar, "id_Autocar", "description", offre.id_Autocar);
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom", offre.id_VilleArrivee);
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom", offre.id_VilleDepart);
            return View(offre);
        }

        public ActionResult Delete(int id)
        {
            if (Session["admin"] != null)
                return RedirectToAction("connexion", "Admins");

            Offre offre = db.Offre.Find(id);
            db.Offre.Remove(offre);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult abonner(int id)
        {
            if (Session["client"] == null)
                return RedirectToAction("connexion", "Clients");
          
            Offre offre = db.Offre.Find(id);
            int idC = ((Client)Session["Client"]).id_Client;
            LigneAbonnement abonnement = new LigneAbonnement();
            abonnement.id_Client = idC;
            abonnement.id_Offre = offre.id_Offre;
            offre.nb_Abonnes_Atteints++;
            db.LigneAbonnement.Add(abonnement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult desabonner(int id)
        {
            if (Session["client"] == null)
                return RedirectToAction("connexion", "Clients");

            Offre offre = db.Offre.Find(id);
            int idC = ((Client)Session["Client"]).id_Client;
            LigneAbonnement abonnement = (LigneAbonnement)db.LigneAbonnement.Where(a => a.id_Client == idC && a.id_Offre == offre.id_Offre);
            offre.nb_Abonnes_Atteints--;
            db.LigneAbonnement.Remove(abonnement);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}