using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.BusinessLayer.Results;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using MyEvernote.Entities.Messages;
using MyEvernote.Entities.ValueObjects;
using MyEvernoteCommon.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class EvernoteUserManager : ManagerBase<EvernoteUsers>
    {
        
        public BusinessLayerResult<EvernoteUsers> RegisterUser(RegisterViewModel data)
        {
            //Kullanıcı username kontrolü aynı olmamalı
            //KUllanıcı E-posta kontrülü
            //kayıt işlemi
            //aktivasyon e-postası gönderimi



            //girilen kullanıcı adının veya emailin ikiisnden birinin veya ikisininde  aynısı var mı kontrolü
            EvernoteUsers user = Find(x => x.Username == data.Username || x.Email == data.EMail);
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            if (user != null)
            {
                //eşleşme sağlanmışsa user null değidlir
                //E-mail yad akullanıcı adı sistemde var

                if (user.Username == data.Username)
                {

                    // layerResult.Errors.Add("kullanıcı adı kayıtlı.");
                    res.AddError(ErrorMessageCode.UsernameAlredyExists, "Kullanıcı adı kayıtlı");
                }

                if (user.Email == data.EMail)
                {
                    res.AddError(ErrorMessageCode.EmailAlredyExists, "E-mail  kayıtlı");
                    // res.Errors.Add("E-mail kayıtlı.");

                }

            }
            else
            {
                int dbResult = base.Insert(new EvernoteUsers()
                {
                    Username = data.Username,
                    Email = data.EMail,
                    ProfileImageFilename = "user.png",
                    Password = data.Password,
                    ActivateGuid = Guid.NewGuid(),

                    IsActive = false,
                    IsAdmin = false,

                });
                if (dbResult > 0)
                {

                    res.Result = Find(x => x.Email == data.EMail && x.Username == data.Username);

                    //TODO:bir aktivasyon maili  atılacak
                    //layerResult.Result.AktivateGuid aktivasyon kodu ile mail atmak
                    string siteUrl = ConfigHelper.Get<string>("SiteRootUrl");
                    string activeUrl = $"{siteUrl}/Home/UserActive/{res.Result.ActivateGuid}";
                    string body = $"Merhaba {res.Result.Username}; <br> <br> Hesabınızı aktifleştirmek için <a href='{activeUrl}' target='_blank'>tıklayınız <a/>.";
                    MailHelper.SendMail(body, res.Result.Email, "MyEvernote Hesap aktifleştirme");

                }
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUsers> LoginUser(LoginViewModel data)
        {

            //Giriş kontrolü ve yönlendirme
            //hesap aktive edilmiş mi?



            //kullanıcı adı ve şifre eşleşiyor mu

            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            res.Result = Find(x => x.Username == data.Username && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive)
                //hata mesajlarını kod vereceğim böylece üzezrinden işlem yaparken kodla hatanın ne olduğunu daha kolay anlayıp farklı deillerdede mesaj verebilirim
                {

                    res.AddError(ErrorMessageCode.UserIsNotActive, "Kullanıcı aktif değildir.");
                    res.AddError(ErrorMessageCode.CheckYourEmail, "Lütfen e-posta adresinin kontrol ediniz");
                    // res.Errors.Add("Kullanıcı aktif değildir.Lütfen e-posta adresinin kontrol ediniz");

                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UsernameOrPassWrong, "Kullanıcı adı yada şifre uyuşmuyor.");
                // res.Errors.Add("Kullanıcı adı yada şifre uuyşmuyor.");
            }
            return res;

        }

        public BusinessLayerResult<EvernoteUsers> ActivateUser(Guid activateId)
        {
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            res.Result = Find(x => x.ActivateGuid == activateId);
            if (res.Result != null)
            {
                if (res.Result.IsActive)
                {
                    res.AddError(ErrorMessageCode.UserAlredyActive, "Kullanıcı zaten aktif edilmiştir.");
                    return res;
                }
                res.Result.IsActive = true;

                Update(res.Result);
            }
            else
            {
                res.AddError(ErrorMessageCode.ActivateIdDoesNotExist, "Aktifleştirilecek kullanıcı bulunamadı.");
            }
            return res;
        }

        public BusinessLayerResult<EvernoteUsers> GetUserById(int id)
        {
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            res.Result = Find(x => x.Id == id);
            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.UserNotFound, "Kullanıcı Bulunamadı.");

            }
            return res;
        }

        public  BusinessLayerResult<EvernoteUsers> UpdateProfile (EvernoteUsers data)
        {
            EvernoteUsers db_user =Find(x => x.Username != data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {

                    // layerResult.Errors.Add("kullanıcı adı kayıtlı.");
                    res.AddError(ErrorMessageCode.UsernameAlredyExists, "Kullanıcı adı kayıtlı");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlredyExists, "E-mail  kayıtlı");
                    // res.Errors.Add("E-mail kayıtlı.");

                }
                return res;

            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
           


            if (string.IsNullOrEmpty(data.ProfileImageFilename) == false)
            {
                res.Result.ProfileImageFilename = data.ProfileImageFilename;
            }
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdate, "Profil Güncellenemedi.");
            }

            return res;
        }

        public BusinessLayerResult<EvernoteUsers> RemoveUserById(int id)
        {
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            EvernoteUsers user = Find(x => x.Id == id);
            if(user != null)
            {
                if(Delete(user)== 0) {

                    res.AddError(ErrorMessageCode.UserCouldRemove, "Kullanıcı silinemedi");
                    return res;
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.UserCouldNotFind, "Kullanıcı bulunamadı");
            }
            return res;
        }
        //NEW ile metot gizleme yaptık metod hiding araştır
        public new BusinessLayerResult<EvernoteUsers> Insert(EvernoteUsers data)
        {
            
            EvernoteUsers user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();

            res.Result = data;
            if (user != null)
            {
                //eşleşme sağlanmışsa user null değidlir
                //E-mail yad akullanıcı adı sistemde var

                if (user.Username == data.Username)
                {

                    // layerResult.Errors.Add("kullanıcı adı kayıtlı.");
                    res.AddError(ErrorMessageCode.UsernameAlredyExists, "Kullanıcı adı kayıtlı");
                }

                if (user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlredyExists, "E-mail  kayıtlı");
                    // res.Errors.Add("E-mail kayıtlı.");

                }

            }
            else
            {
                res.Result.ProfileImageFilename = "user.png";
                res.Result.ActivateGuid = Guid.NewGuid();

             //Insert(res.Result);//Bu ınsert yeni metoto gizleme ile yaptığımız insert
               if(base.Insert(res.Result) == 0)
                {
                    //Bu şekilde base insert olduğunu söylüyoruz
                    res.AddError(ErrorMessageCode.UserCouldNotInserted, "Kullanıcı Eklenmedi");
                }
            }

            return res;


        }

        public new BusinessLayerResult<EvernoteUsers> Update(EvernoteUsers data)
        {
            EvernoteUsers db_user = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<EvernoteUsers> res = new BusinessLayerResult<EvernoteUsers>();
            res.Result = data;
            if (db_user != null && db_user.Id != data.Id)
            {
                if (db_user.Username == data.Username)
                {

                    // layerResult.Errors.Add("kullanıcı adı kayıtlı.");
                    res.AddError(ErrorMessageCode.UsernameAlredyExists, "Kullanıcı adı kayıtlı");
                }

                if (db_user.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlredyExists, "E-mail  kayıtlı");
                    // res.Errors.Add("E-mail kayıtlı.");

                }
                return res;

            }
            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;
            res.Result.Username = data.Username;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
          

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.UserCouldNotUpdate, "Kullanıcı Güncellenemedi.");
            }

            return res;
        }

    }

}