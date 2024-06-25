using MyEvernote.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class RepositoryBase
    {

        protected static DatabaseContext context;
        private static object _lockSync=new object();

        protected RepositoryBase()
        {
          CreateContext();
        

        }
        //Bİr nesne tanımlicam singleton olarka herkes onu kullanacak yeni nesne tanımlamayacak demek için singleton kullanılır
        private static void CreateContext() // sttaic metotlar new lemeden çalışır
        {//Her defasında DatabaseContext i new lemesin bir defa new lesin ve onu kullansın null olmadığı durumlarda null sa newlesin
            if (context == null)
                //_db eğer null se db yi new le bana geri dön
            {
                lock (_lockSync)
                {
                    //kitleme if eiki tane iş girerse sadec birin yapar
                    if ( context == null )
                    {
                        //garantiye almak için
                        context = new DatabaseContext();
                    }
                   
                }
            }

                
         
        }
    }
}
