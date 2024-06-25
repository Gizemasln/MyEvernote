using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    //Note ve user arasınca like için çoka çok ilişkisi olduğu için araya bir referans tablo eklemem gerekiyor
    //Bir notu birden fazla kullanıcı like yapabilir 
    //bir kullanıcı bir fazla nota like yapabilir
    [Table("Likes")]
    public class Liked
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
         //Bağlantı için virtual taımlıyorum
        //Notun kendisi
        public virtual Note Note { get; set; }
        //Notu likelayan kullanıcı
        public virtual EvernoteUsers LikedUser { get; set; }
    }
}
