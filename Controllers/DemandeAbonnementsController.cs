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
    public class DemandeAbonnementsController : Controller
    {
        private Gestion_NavettesEntities1 db = new Gestion_NavettesEntities1();


        // GET: DemandeAbonnements
        public ActionResult Index()
        {
            int id = ((Client)Session["client"]).id_Client;
            var demandeAbonnement = db.DemandeAbonnement.Include(d => d.Client).Include(d => d.Ville).Include(d => d.Ville1).Where(r => r.id_Client == id);
            return View(demandeAbonnement.ToList());
        }
        public ActionResult AdminIndex()
        {
            if (Session["client"] != null)
                return RedirectToAction("connexion", "Clients");
            
                var demandes = db.DemandeAbonnement.Include(a => a.Client).Include(a => a.Ville).Include(d => d.Ville1);
                return View(demandes);
        }

        // GET: DemandeAbonnements/Details/5
        public ActionResult Details(int id)
        {
            DemandeAbonnement demandeAbonnement = db.DemandeAbonnement.Find(id);
            if (demandeAbonnement == null)
            {
                return HttpNotFound();
            }
            return View(demandeAbonnement);
        }

        // GET: DemandeAbonnements/Create
        public ActionResult Create()
        {
            if (Session["client"] == null)
                return RedirectToAction("connexion", "Clients");
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom");
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom");
            return View();
        }

        // POST: DemandeAbonnements/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DemandeAbonnement demandeAbonnement)
        {
            if (Session["client"] == null)
                return RedirectToAction("connexion", "Clients");
            if (ModelState.IsValid
               && demandeAbonnement.heure_Arrivee != demandeAbonnement.heure_Depart
               && demandeAbonnement.heure_Arrivee > demandeAbonnement.heure_Depart
               && demandeAbonnement.date_Depart <= demandeAbonnement.date_Arrivee
               && demandeAbonnement.date_Depart >= demandeAbonnement.date_Demande
               )
            {
                demandeAbonnement.date_Demande = DateTime.Now;
                demandeAbonnement.id_Client = ((Client)Session["client"]).id_Client;
                db.DemandeAbonnement.Add(demandeAbonnement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom", demandeAbonnement.id_VilleArrivee);
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom", demandeAbonnement.id_VilleDepart);
            return View(demandeAbonnement);
        }

        // GET: DemandeAbonnements/Edit/5
        public ActionResult Edit(int id)
        {
            if (Session["client"] == null)
                return RedirectToAction("connexion", "Clients");

            DemandeAbonnement demandeAbonnement = db.DemandeAbonnement.Find(id);
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom", demandeAbonnement.id_VilleArrivee);
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom", demandeAbonnement.id_VilleDepart);
            return View(demandeAbonnement);
        }

        // POST: DemandeAbonnements/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Demande,heure_Depart,heure_Arrivee,date_Demande,description,prix,date_Depart,date_Arrivee,id_Client,id_VilleDepart,id_VilleArrivee")] DemandeAbonnement demandeAbonnement)
        {
            if (ModelState.IsValid
                && demandeAbonnement.date_Depart != demandeAbonnement.date_Arrivee
                && demandeAbonnement.heure_Arrivee != demandeAbonnement.heure_Depart
                && demandeAbonnement.heure_Arrivee > demandeAbonnement.heure_Depart
                && demandeAbonnement.date_Depart < demandeAbonnement.date_Arrivee
                )
            {
                // demandeAbonnement.date_Demande = DateTime.Now;
                demandeAbonnement.id_Client = ((Client)Session["client"]).id_Client;
                db.Entry(demandeAbonnement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.id_VilleArrivee = new SelectList(db.Ville, "id_Ville", "nom", demandeAbonnement.id_VilleArrivee);
            ViewBag.id_VilleDepart = new SelectList(db.Ville, "id_Ville", "nom", demandeAbonnement.id_VilleDepart);
            return View(demandeAbonnement);
        }


        public ActionResult Delete(int id)
        {
            if (Session["societe"] != null)
                return RedirectToAction("connexion", "Societes");

            else if (Session["client"] == null || Session["admin"] == null)
            {
                DemandeAbonnement demandeAbonnement = db.DemandeAbonnement.Find(id);
                db.DemandeAbonnement.Remove(demandeAbonnement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public ActionResult accepter(int id)
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            DemandeAbonnement demande = db.DemandeAbonnement.Find(id);

            int idS = ((Societe)Session["Societe"]).id_Societe;

             Offre offre  = new Offre();
            offre.id_Societe = idS;
            offre.date_Offre = DateTime.Now;
            offre.nb_Abonnes_Atteints = 1;
            offre.date_Depart = demande.date_Depart;
            offre.date_Arrivee = demande.date_Arrivee;
            offre.heure_Arrivee = demande.heure_Arrivee;
            offre.heure_Depart = demande.heure_Depart;
            offre.description = demande.description;
            offre.prix = demande.prix ;
            offre.id_Autocar = 1;
            offre.nb_Abonnes_Voulus = 6;
            offre.id_VilleArrivee = demande.id_VilleArrivee;
            offre.id_VilleDepart = demande.id_VilleDepart;

                db.Offre.Add(offre);
                db.SaveChanges();       
            ViewBag.id_Autocar = new SelectList(db.Autocar, "id_Autocar", "marque", offre.id_Autocar);

                return RedirectToAction("Index");

        }
    }
}