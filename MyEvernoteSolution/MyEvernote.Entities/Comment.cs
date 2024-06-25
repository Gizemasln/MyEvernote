using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Comments")]
    public class Comment : MyEntityBase
    {
        //nota Yapılan yorumlar
        [Required, StringLength(300)]
        public string Text { get; set; }
        //Hangi nota yorum olarak yapıldı bir tane yorum bir tane nota yapılır
        public virtual Note Note { get; set; }
        //Yorumu yapan kişi bir yorumu bir kişi yapar
        public virtual EvernoteUsers Owner { get; set; }
    }
}
