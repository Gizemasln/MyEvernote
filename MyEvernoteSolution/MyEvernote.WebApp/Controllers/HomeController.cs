using MyEvernote.BusinessLayer;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernote.WebApp.Filters;
using MyEvernote.WebApp.Models;
using MyEvernote.WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Mvc;

namespace MyEvernote.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private NoteManager noteManager = new NoteManager();
        private CategoryManager categoryManager = new CategoryManager();
        private EvernoteUserManager evernoteUserManager = new EvernoteUserManager();
        // GET: Home
        public ActionResult Index()
        {
            //KENDİM HATA GÖNDERMEK İSTEDİM;
           // throw new Exception("Herhangi bir hata oluştu.");


            //Categorycontoller üzerinden gelen view talebi ve modeli

            //if (TempData["mm"]!= null)

            //{
            //    return View(TempData["mm"] as List<Note>);
            //}


            return View(noteManager.ListQueryable().Where(x => x.IsDraft == false).OrderByDescending(x => x.ModifiedOn).ToList());
            //IsDraft(taslak olmayanalrı sadece görüntüle
            //   return View(noteManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
            //OrderByDescending , ModifiedOn ı küçükte büyüğe sıralayıp gönderecek
            //  return View(nm.GetSAllNoteQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }
        public ActionResult ByCategory(int? id)
        {
            if(id == null)
            {
                //Hata mesajı tarzı şeyler göndermek için
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest );
            }

            List<Note> notes = noteManager.ListQueryable().Where(x => x.IsDraft == false && x.CategoryId == id).OrderByDescending(x => x.ModifiedOn).ToList();
            //Notesten ürettiğim bu modeli index view in de görrüntüle IsDraft(taslak olmayanalrı sadece)
            return View("Index",notes);
        }
        public ActionResult MostLiked() {
            //EN beğenilen notlar
            //OrderByDescending büyükten küçüğe
            //Not tarihi sıralamakla aynı 

            return View("Index",noteManager.ListQueryable().OrderByDescending(x => x.LikeCount).ToList());
        
        }
        public ActionResult About()
        {
            return View();
        }
        [Auth]
        public ActionResult ShowProfile()
        {
         
            BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                //Kullanıcıyı bir hata ekranına yönlendirmemiz gerekiyor
                //ErrorViewModel ını çalıştır modelinide erorViewModel olarak
                //Error Actiona  gitmicek erorr viewına gidecek 
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error",errornotifyObj);
            }
            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {
       
            BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.GetUserById(CurrentSession.User.Id);
            if (res.Errors.Count > 0)
            {
                //Kullanıcıyı bir hata ekranına yönlendirmemiz gerekiyor
                //ErrorViewModel ını çalıştır modelinide erorViewModel olarak
                //Error Actiona  gitmicek erorr viewına gidecek 
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Hata oluştu",
                    Items = res.Errors
                };
                return View("Error", errornotifyObj);
            }
            return View(res.Result);
        }
        [HttpPost]
        public ActionResult EditProfile(EvernoteUsers model, HttpPostedFileBase ProfileImage)
        {
            //EDitProfile sayfasında inputun name ile HttpPostedFileBase nin parametre ismiinin aynı olması gerkeiyor
            ModelState.Remove("ModifiedUsername");
            if (ModelState.IsValid)
            {

                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpeg"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";
                    ProfileImage.SaveAs(Server.MapPath($"~/images/{filename}"));
                    model.ProfileImageFilename = filename;

                }
                BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.UpdateProfile(model);
                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errornotifyObj = new ErrorViewModel()
                    {
                        Items = res.Errors,
                        Title = "Profil Güncellendi.",
                        RedirectingUrl = "/Home/EditProfile"
                    };

                    return View("Error", errornotifyObj);
                }
                CurrentSession.Set<EvernoteUsers>("login",res.Result);//profil güncellendiği için session güncellendi
                return RedirectToAction("ShowProfile");
            }
            return View(model);

        }
        [Auth]
        public ActionResult DeleteProfile()
        {
          
            BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.RemoveUserById(CurrentSession.User.Id);
            if(res.Errors.Count > 0)
            {
                ErrorViewModel messages = new ErrorViewModel()
                {
                    Items = res.Errors,
                    Title = "Profil Silinemedi",
                    RedirectingUrl = "/Home/ShowProfile/"
                };
                return View("Error" ,messages);
            }
            Session.Clear();
            return RedirectToAction("Index");
           
        }   
        public ActionResult Login() { 
          
            
            return View(); }
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //yönlendirme

            if (ModelState.IsValid)
            {
         
                BusinessLayerResult<EvernoteUsers> res = evernoteUserManager.LoginUser(model);
                if (res.Errors.Count > 0)
                {
                    if (res.Errors.Find(x => x.Code == Entities.Messages.ErrorMessageCode.UserIsNotActive) != null)//Error mesajlarına kod verip soruna hangisi gelmesini gerekiyorsa o mesajı getirdik
                    {
                        ViewBag.SetLink = "http://Home/Active/1234-567-78980";
                    }

                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                 

                    return View(model);
                }
                //Session' a kullanıcı bilgi saklama
                CurrentSession.Set<EvernoteUsers>("login", res.Result);
                return RedirectToAction("Index");
            }
            return View(model);
        
        }       
        public ActionResult Register()
        {


            return View();
        }
        [HttpPost]
        public ActionResult Register(RegisterViewModel model)
        { //Kullanıcı username kontrolü aynı olmamalı
            //KUllanıcı E-posta kontrülü
            //kayıt işlemi
            //aktivasyon e-postası gönderimi
            if (ModelState.IsValid) //Bütün alanlar doğru giirldiyse içeriye girip iki if buloğunu inceleyecek
            {
                //EvernoteUserManagerdeki e posta veya kullanıc adı olan hata mesajını yakalamak için
             
                BusinessLayerResult<EvernoteUsers> res =evernoteUserManager.RegisterUser(model);

                if(res.Errors.Count > 0)
                {

                    res.Errors.ForEach(x => ModelState.AddModelError("",x.Message));

                    return View(model);
                }

                //EvernoteUsers user = null;
                //try
                //{
                //    user=eum.RegisterUser(model);
                //}
                //catch (Exception ex)
                //{
                //    ModelState.AddModelError("" ,ex.Message);
                //    //gelen mesajı olduğu gibi model erorr e verecem

                //}



                //if (model.Username == "aaa")
                //{
                //    ModelState.AddModelError("", "Kullanıcı adı kullanıulıyor.");

                //}
                //if(model.EMail =="adasdas.com" )
                //{

                //    ModelState.AddModelError("", "E-posta kullanıulıyor.");

                //}
                //foreach (var item in ModelState)
                //{
                //    if (item.Value.Errors.Count > 0)
                //    {
                //        return View(model);


                //    }
                //}
                //if(user == null) { 

                //return View(model);
                //}
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "kayıt bAşarılı",
                    RedirectingUrl="/Home/Login",           
                };
                notifyObj.Items.Add("Lütfen e-posta adresini gönderdiğimiz aktivasyon linkine tıklayarak hesabını aktive ediniz.Hesabınızı aktive etmeden not ekleyemez ve beğeni yapamazsınız.");
                return View("Ok",notifyObj);

                // return RedirectToAction("RegisterOk");
                        }
           


            return View(model);
        }
        public ActionResult RegisterOk()
        {
            //BU ACTİON U SİLİYOIRUM GEREK KALMADI
            //REGİSTEROK VİEW İNİDE SİLDİM VE REGİSTERİN YÖNLENDİRMESİNİ DEĞİŞTİRDİM
            return View();
        }    
        public ActionResult UserActivate(Guid id) {
            //kullanıcı aktivasyonu sağlanacak
         
           BusinessLayerResult<EvernoteUsers> res =evernoteUserManager.ActivateUser(id);
            if(res.Errors.Count > 0)
            {
                ErrorViewModel errornotifyObj = new ErrorViewModel()
                {
                    Title = "Geçersiz İşlem",
                    Items = res.Errors
                };
              //  TempData["errors"] = res.Errors;

                return View("Error", errornotifyObj);
               //return RedirectToAction("UserActivateCancel");


            }
            OkViewModel oknotifyObj = new OkViewModel()
            {
                Title = "Hesap Aktifleşitirldi",
                RedirectingUrl = "/Home/Login",
           

            };
            oknotifyObj.Items.Add("Hesabınız aktifleştiirldi.Artık not paylaşabilir ve beğenme yapabilirsiniz..");
            return View("Ok",oknotifyObj);
           // return RedirectToAction("UserActivateOk");
        }
        //public ActionResult UserActivateOk()
        //{
        //VİEW ını sildim
        //    //kullanıcı aktivasyonu sağlanacak
        //    return View();
        //}
        //public ActionResult UserActivateCancel()
        //{
        //VİEW ını sildim
        //    List<ErrorMessageObj> errors = null;
        //    if (TempData["errors"] != null)
        //    {
        //    errors = TempData["errors"] as List<ErrorMessageObj>;


        //    }
        //    return View(errors);
        //}
        public ActionResult Logout()
        {

            Session.Clear();

            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied() {
            return View();
                }

        public ActionResult HasError()
        {
            return View();
        }
    }
}