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
    public class ClientsController : Controller
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

        // GET: Clients
        public ActionResult Index()
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");

            return View(db.Client.ToList());
        }
        [Route("Client/inscription")]

        public ActionResult inscription()
        {
            return View();
        }
        [Route("Client/inscription")]
        [HttpPost]
        public ActionResult inscription(Client client)
        {
            if (ModelState.IsValid && IsValidEmailAddress(client.email))
            {
                client.date_ajout = DateTime.Now;
                db.Client.Add(client);
                db.SaveChanges();
                return RedirectToAction("connexion");
            }
            return View(client);

        }

        [Route("Client/connexion")]
        public ActionResult connexion()
        {
            return View();
        }

        [Route("Client/connexion")]
        [HttpPost]
        public ActionResult connexion(Client client)
        {
            var existe = db.Client.Where(a => a.email == client.email && a.password == client.password).FirstOrDefault();
            if (existe != null)
            {
                Session["client"] = existe;
                return RedirectToAction("Index", "Home");
            }
            else return View();
        }
        // GET: Clients
        // /Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
                return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_Client,email,password,role,nom,prenom,date_ajout")] Client client)
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid && IsValidEmailAddress(client.email))
                {
                    db.Client.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Client.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // Pour vous protéger des attaques par survalidation, activez les propriétés spécifiques auxquelles vous souhaitez vous lier. Pour 
        // plus de détails, consultez https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id_Client,email,password,role,nom,prenom,date_ajout")] Client client)
        {

            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                if (ModelState.IsValid && IsValidEmailAddress(client.email))
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(client);
            }
        }

        public ActionResult Delete(int id)
        {
            if (Session["admin"] == null)
                return RedirectToAction("connexion", "Admins");
            else
            {
                Client client = db.Client.Find(id);
                db.Client.Remove(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
        public ActionResult deconnexion()
        {

            if (Session["client"] == null)
                return RedirectToAction("connexion", "Clients");
            Session["client"] = null;
            Session.Contents.RemoveAll();
            return RedirectToAction("connexion", "Clients");
        }
    }
}
