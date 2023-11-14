using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.WebApp.Filters;

namespace MyEvernote.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class EvernoteUsersController : Controller
    {
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
   
        public ActionResult Index()
        {
            return View(evernoteUserManager.List());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUsers evernoteUsers = evernoteUserManager.Find(x => x.Id == id.Value);
            if (evernoteUsers == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUsers);
        }

   
        public ActionResult Create()
        {
            return View();
        }

     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EvernoteUsers evernoteUsers)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.Insert(evernoteUsers);
               
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUsers);
                }
                return RedirectToAction("Index");
            }
            return View(evernoteUsers);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            EvernoteUsers evernoteUser = evernoteUserManager.Find(x => x.Id == id.Value);

            if (evernoteUser == null)
            {
                return HttpNotFound();
            }

            return View(evernoteUser);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EvernoteUsers evernoteUser)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.Update(evernoteUser);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(evernoteUser);
                }

                return RedirectToAction("Index");
            }
            return View(evernoteUser);
        }


        // GET: EvernoteUsers/Delete/5

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EvernoteUsers evernoteUsers = evernoteUserManager.Find(x => x.Id == id.Value);
            if (evernoteUsers == null)
            {
                return HttpNotFound();
            }
            return View(evernoteUsers);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EvernoteUsers evernoteUsers = evernoteUserManager.Find(x => x.Id == id);
            evernoteUserManager.Delete(evernoteUsers);
            return RedirectToAction("Index");
        }

    }
}
