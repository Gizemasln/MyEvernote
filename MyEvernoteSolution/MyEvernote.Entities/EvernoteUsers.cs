using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("EvernoteUsers")]
    public class EvernoteUsers : MyEntityBase
    {
    //MyEntityBase sınıfından referans olındı 
    //İd myentitybase den geldiği için bu classa yazmıyorum
       // public readonly object AktivateGuid;

        [DisplayName("İsim"),StringLength(25,ErrorMessage ="{0} alanı max. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyad"), StringLength(25 )]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı adı"), Required(ErrorMessage ="{0} alanı gereklidir.") ,StringLength(25, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-Posta"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(25,ErrorMessage = "{0} alanı max. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength (30),ScaffoldColumn(false)] //images/user_12.jpg
       // ScaffoldColumn(false) = view üretim esnasında işlenmesini istemiyorum
        public string ProfileImageFilename { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        //Kullanıcının admin oluğ olmadığının kontrolü sağlanıyor
        [DisplayName("Yönetici")]
        public bool IsAdmin { get; set; }

         //Activate id sinin daha zor bilinmesi için
        [Required, ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        //Bir kullanıcının birden çok kendi oluşturduğu notu var
        public virtual List<Note> Notes { get; set; }
         //Bir kullanıcının birden çok kendi oluşturduğu yorumu var
        public virtual List<Comment> Comments { get; set; }
          //Bir kullanıcının birden çok kendi like yaptığı not vardır
        public virtual List<Liked> Likes { get; set; }
    }
}
