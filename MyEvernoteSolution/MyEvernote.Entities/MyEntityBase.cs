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
    public class MyEntityBase
    {
        //Bu classtaki tüm özellikleri referans ile diğer classlarda kullanabiliyoruz bu sayede 
        //her defasında yazmamıza gerek kalmıyor
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [DisplayName("Oluşturma Tarihi"),ScaffoldColumn(false),Required]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Güncelleme Tarihi"), ScaffoldColumn(false), Required]
        public DateTime ModifiedOn { get; set; }
        [DisplayName("Güncelleyin"), ScaffoldColumn(false), Required,StringLength(30)]
        public string ModifiedUsername { get; set; }
    }
}
