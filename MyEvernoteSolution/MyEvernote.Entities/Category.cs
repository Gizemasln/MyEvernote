﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities
{
    [Table("Categories")]
    public class Category : MyEntityBase
    {
        [DisplayName("Kategori"),Required(ErrorMessage ="{0} Alanı Gereklidir."), StringLength(50,ErrorMessage = "{0} Alanı  ma. {1} Karakterdir.")]
        public string Title { get; set; }
        [DisplayName("Açıklama"), StringLength(150, ErrorMessage = "{0} Alanı  ma. {1} Karakterdir.")]
        public string Description { get; set; }

        //Category ve note classını bağlantılıyorum  virtual olarak tanımlıyorum
        //Bir kategorinin birden fazla notu olabileceği için list özelliğini ekliyorum
        public virtual List<Note> Notes { get; set; }

        public Category() { 
        
        //otomatik oalrak not listesini oluşturması için
            Notes = new List<Note>();
        }
    } 
}
