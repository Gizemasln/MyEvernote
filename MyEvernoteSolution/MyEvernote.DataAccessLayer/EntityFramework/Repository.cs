using MyEvernote.Core.DataAccess;
using MyEvernote.DataAccessLayer;
using MyEvernote.Entities;
using MyEvernoteCommon;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    
    public class Repository<T> : RepositoryBase , IDataAccess<T> where T: class
    {
  
        private DbSet<T> _objectSet;
        public Repository()
        {
         
            _objectSet = context.Set<T>();
        }
        public List<T> List()
        {
            //T nin sadece class olamsı gerektiğni bildirmek üçün where T: class ile kısıt koyduk
           return _objectSet.ToList();
        }
        public IQueryable<T> ListQueryable() 
        { 
            return _objectSet.AsQueryable<T>(); 
        }
        public List<T> List(Expression<Func<T,bool>> where)
        {
            //istenilen şartlarda veri getirmek için where ile şart koşuyorum
            
            return _objectSet.Where(where).ToList();

        }
        public int Insert(T obj)
        {
            //eklenecek olan verinin ne türde geleceğini bilmediğim için T obj şeklinde tanımladım
          _objectSet.Add(obj);
            if(obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;
                DateTime now = DateTime.Now;
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUsername = App.Common.GetCurrentUsername(); //TODO:işlem yapan kullanıcı adı yazılamlı

            }
            return Save();
        }
        public int Update(T obj)
        {
        
            if (obj is MyEntityBase)
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUsername = App.Common.GetCurrentUsername(); //TODO:işlem yapan kullanıcı adı yazılamlı
            }
                return Save();
        }
        public int Delete(T obj)
        {

            //if (obj is MyEntityBase)
            //{
            //    MyEntityBase o = obj as MyEntityBase;

            //    o.ModifiedOn = DateTime.Now;
            //    o.ModifiedUsername = App.Common.GetCurrentUsername(); //TODO:işlem yapan kullanıcı adı yazılamlı
            //}
            _objectSet.Remove(obj);
            return Save();
        }
        public int Save()
        {
            //yapılan işlemden kaç adet verinin etkilendiğini göstermek için
            return context.SaveChanges();
        }
        public T Find(Expression<Func<T, bool>> where)
        {
            //seçtiğim şartlata uygun olan tek bir veriyi döndürmek için liste değil tek veri 
            return _objectSet.FirstOrDefault(where);
        }

        
    }
}
