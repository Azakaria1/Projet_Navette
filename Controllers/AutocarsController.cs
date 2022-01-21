using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Projet_Navette.Models;

namespace Projet_Navette.Controllers
{
    public class AutocarsController : Controller
    {
        private Gestion_NavettesEntities1 db = new Gestion_NavettesEntities1();

        // GET: Autocars
        public ActionResult Index()
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            else
            {
                int id = ((Societe)Session["societe"]).id_Societe;
                var autocar = db.Autocar.Where(a => a.id_Societe == id);
                return View(autocar.ToList());
            }
        }
        public ActionResult AdminIndex()
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                var autocar = db.Autocar.Include(a => a.Societe);
                return View(autocar);
            }
        }

        // GET: Autocars/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autocar autocar = db.Autocar.Find(id);
            if (autocar == null)
            {
                return HttpNotFound();
            }

            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            else
                return View(autocar);
        }

        // GET: Autocars/Create
        public ActionResult Create()
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            else
            {
                return View();
            }
        }

        // POST: Autocars/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Autocar autocar, HttpPostedFileBase imageChoisie)
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            else
            {
                if (imageChoisie != null)
                {
                    string FileExtension = Path.GetExtension(imageChoisie.FileName);
                    string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.fff") + FileExtension;
                    string UploadPath = "~/Images/";
                    imageChoisie.SaveAs(Server.MapPath(UploadPath + FileName));
                    autocar.imagePath = FileName;
                }
                ModelState.Remove("imagePath");
                if (ModelState.IsValid)
                {
                    int id = ((Societe)Session["societe"]).id_Societe;

                    autocar.id_Societe = id;
                    db.Autocar.Add(autocar);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(autocar);
            }
        }

        // GET: Autocars/Edit/5
        public ActionResult Edit(int id)
        {
            Autocar autocar = db.Autocar.Find(id);
            if (autocar == null)
            {
                return HttpNotFound();
            }

            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            else
            {
                return View(autocar);
            }
        }

        // POST: Autocars/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Autocar autocar, HttpPostedFileBase imageChoisie)
        {

            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");
            else
            {
                if (imageChoisie != null)
                {
                    string FileExtension = Path.GetExtension(imageChoisie.FileName);
                    string FileName = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss.fff") + FileExtension;
                    string UploadPath = "~/Images/";
                    string ancienne = Request.MapPath(autocar.imagePath);
                    if (System.IO.File.Exists(ancienne))
                        System.IO.File.Delete(ancienne);
                    imageChoisie.SaveAs(Server.MapPath(UploadPath + autocar.imagePath));
                    autocar.imagePath = FileName;
                }
                ModelState.Remove("imagePath");
                if (ModelState.IsValid)
                {
                    int id = ((Societe)Session["societe"]).id_Societe;
                    autocar.id_Societe = id;
                    db.Entry(autocar).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(autocar); ;
            }
        }
        public ActionResult Delete(int id)
        {
            if (Session["societe"] == null)
                return RedirectToAction("connexion", "Societes");

            int idd = ((Societe)Session["societe"]).id_Societe;

            Autocar autocar = db.Autocar.Find(id);
            if (autocar.id_Societe != idd)
                return RedirectToAction("connexion", "Societes");

            System.IO.File.Delete(autocar.imagePath);
            db.Autocar.Remove(autocar);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}