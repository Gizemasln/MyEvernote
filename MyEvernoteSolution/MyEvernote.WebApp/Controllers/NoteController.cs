using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.MappingViews;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using MyEvernote.BusinessLayer;
using MyEvernote.Entities;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;

namespace MyEvernote.WebApp.Controllers
{
    [Exc]
    public class NoteController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private LikedManager likedManager = new LikedManager();

        // GET: Note
        [Auth]
        public ActionResult Index()
        {
            var notes = noteManager.ListQueryable().Include("Category").Include("Owner").Where(
                x => x.Owner.Id == CurrentSession.User.Id).OrderByDescending(
                x => x.ModifiedOn);

            return View(notes.ToList());
        }

        //SADECE BEĞENDİKLERİMİ GETİREN ACTİON FİLTRELEME YAPMAK İÇİN
        [Auth]
        public ActionResult MyLikedNotes()
        {             
            var notes = likedManager.ListQueryable().Include("LikedUser").Include("Note").Where(x=>x.LikedUser.Id == CurrentSession.User.Id).Select(x=>x.Note).Include("Category").Include("Owner").OrderByDescending(x=>x.ModifiedOn);

            return View("Index",notes.ToList());
        }
        [Auth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x=>x.Id==id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [Auth]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title");
            return View();
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");

            if (ModelState.IsValid)
            {
                note.Owner = CurrentSession.User;
                noteManager.Insert(note);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [Auth]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(),
                "Id", "Title", note.CategoryId);
            return View(note);
        }

        [Auth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Note note)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {
              Note db_note = noteManager.Find(x=>x.Id == note.Id);
                db_note.IsDraft = note.IsDraft;//bu sayfada gelecek = Isdraft
                db_note.CategoryId=note.CategoryId;
                db_note.Text = note.Text;
                db_note.Title = note.Title;
                noteManager.Update(db_note);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(CacheHelper.GetCategoriesFromCache(), "Id", "Title", note.CategoryId);
            return View(note);
        }

        [Auth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        [Auth]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = noteManager.Find(x => x.Id == id);
       noteManager.Delete(note);
            return RedirectToAction("Index");
        }

        //Zaten JS tarafında login kontrolü yapılacak
        [HttpPost]
        public ActionResult GetLiked(int[] ids)
        {
            //gelen ids değerinin not.ıd ile aynı oalnalrı ve kullanıcısıda bu olanları getir
            //kullanıcı tarafından like lannmış kayıtlar
            //Select ile sadece notların id sini verdiğin bir liste elde etmek için
              List<int> likedNoteIds = likedManager.List( 
            x =>x.LikedUser.Id == CurrentSession.User.Id && ids.Contains(
            x.Note.Id)).Select(
            x => x.Note.Id).ToList();
              return Json(new {result = likedNoteIds});
        //    if (CurrentSession.User != null)
        //    {
        //        int userid = CurrentSession.User.Id;
        //        List<int> likedNoteIds = new List<int>();

        //        if (ids != null)
        //        {
        //            likedNoteIds = likedManager.List(
        //                x => x.LikedUser.Id == userid && ids.Contains(x.Note.Id)).Select(
        //                x => x.Note.Id).ToList();
        //        }
        //        else
        //        {
        //            likedNoteIds = likedManager.List(
        //                x => x.LikedUser.Id == userid).Select(
        //                x => x.Note.Id).ToList();
        //        }

        //        return Json(new { result = likedNoteIds });
        //    }
        //    else
        //    {
        //        return Json(new { result = new List<int>() });
        //    }
        }
        //Zaten JS tarafında login kontrolü yapılacak

        [HttpPost]
        public ActionResult SetLikeState(int noteid,bool liked)
        {
            int res = 0;
            if (CurrentSession.User == null)
                return Json(new { hasError = true, errorMessage = "Beğenme işlemi için giriş yapmalısınız.", result = 0 });

            
            //KUllanıcının like yapmaya çalıştığı not verş tabanında var mı;
            Liked like = likedManager.Find(x => x.Note.Id == noteid && x.LikedUser.Id == CurrentSession.User.Id );
            //Veri tabanından notuda almak için;
            Note note = noteManager.Find(x => x.Id == noteid);

            if(like != null && liked == false)
            {
                //Eğer like lanmışsa like dolu gelmişse ve liked false ise(like lanmış = false)

               res= likedManager.Delete(like); //like sil
                //başarılıysa res =1 başarısızsa =0
            }
            else if(like == null && liked == true)
            {
                //like işlemi yok ilk defa like yapılacak liked true yanı boş 
                res = likedManager.Insert(new Liked()
                {
                    LikedUser = CurrentSession.User,
                    Note = note
                });
            }
            if(res > 0)
            {
                //eğer bir işlme yaptıysam res in deeri 1 geldiyse
                if (liked)
                {
                    //like işlemi yaptıysam veri tabanında ki like count değerini 1 arttırmams gerekir
                    note.LikeCount++;
                }
                else
                {
                    //aksi halde like kaldırmışsan veri tabanın daki likme count değğerini 1 azaltmam gerkeir
                    note.LikeCount--;
                }

                //İşlemlerden sonra notu update etmem gerekiyor
              res = noteManager.Update(note);
                //İşlem başarılı olduysa
                return Json(new { hasError = false, errorMessage = string.Empty,
                    result = note.LikeCount });
            }
            //İşlemin başarıısz olması durumu
   
            return Json(new { hasError = true , errorMessage ="Beğenme İşlemi Gerçekleşemedi.",result = note.LikeCount});
        }

       public ActionResult GetNoteText(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = noteManager.Find(x => x.Id == id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return PartialView("_PartialNoteText",note);

        }
    }


        
    }

