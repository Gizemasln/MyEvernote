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
    [Table("Notes")]
    public class Note : MyEntityBase
    {
    //Id ve diğer özellijlerin gelmesi için Myentitybase den referans alıyor
        [DisplayName("Not Başlığı"),Required, StringLength(60)]
        public string Title { get; set; }
        [DisplayName("Not Metni"), Required, StringLength(2000)]
        public string Text { get; set; }
        [DisplayName("Taslak")]
        //Taslak mı 
        public bool IsDraft { get; set; }
        
        [DisplayName("Beğenilme")]
        public int LikeCount { get; set; }
        [DisplayName("Kategori")]
        public int CategoryId { get; set; }//classın adını ve property nin adını yanyan ayazınca otomatik olarak algılıyor

        //Bir notun bir tane sahibi oluşturan kişi var
        public virtual EvernoteUsers Owner { get; set; }
         //Bir notun bir tane kategorisi var
        public virtual Category Category { get; set; }
        //Bir notun birden fazla yorumu var (List)
        public virtual List<Comment> Comments { get; set; }
        //Bir notun birden fazla like var (List)
        public virtual List<Liked> Likes { get; set; }

        public Note() { 
        
            Comments = new List<Comment>();
            Likes = new List<Liked>();
         }
    }
}
