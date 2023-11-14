using MyEvernote.BusinessLayer.Abstract;
using MyEvernote.DataAccessLayer.EntityFramework;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer
{
    public class CategoryManager : ManagerBase<Category>
    {
        //2. yöntem kod yazmadan veritabanından çözmek:
        //VERİTABANI DİAGRAMINDA TABLOLAR ARASINDAKİ INSERT AND UPDATE KURALINI CASCADE YAPMALISIN
    //    public override int Delete(Category category)
    //    {
    //        //OVERRİDE ETMEK İSTEDİĞİN METOT VİRTAUL OLMASI GEREKİYOR
    //        //REPOSİTORYDEN DELTE METODUNU VİRTUAL YAPTIM
    //        //KATEGORİ SİLERKEN KATEGORİLER NOTLARLA BAĞLANTILI OLDUĞU İÇİN SİLMİYOR ÇÖZÜM::
    //        //KATEGORİ İLE İLİŞKİLİ NOTLARIN SİLİNMESİ GEREKİYOR
    //        NoteManager noteManager = new NoteManager();
    //        LikedManager likedManager = new LikedManager();
    //        CommentManager commentManager = new CommentManager();
    //        foreach(Note note in category.Notes.ToList())
    //        {
    //            //döngü dönerken silme yapacağı için listede hata verecek
    //            //bu hatayı enlgelleme için tolist ile listeyi yeni halibne yarlayıp yeni listeye işlem yaptırıyoruz

    //            //Not ile ilişkili likelri sil önce
    //            foreach (Liked like in note.Likes.ToList())
    //            {
    //                likedManager.Delete(like);
    //            }
    //            //NOte ile ilişkili commentlerin silinmesi
    //            foreach(Comment comment in note.Comments.ToList())
    //                {
                    
    //                commentManager.Delete(comment);
    //            }
    //            noteManager.Delete(note);


    //        }
    //        return base.Delete(category);
    //    }
    }
}
