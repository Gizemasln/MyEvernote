﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Core.DataAccess
{
    public interface IDataAccess<T>
    {
        //Interface de kod olmaz sadece metot tanımı olur
         List<T> List();
        IQueryable<T> ListQueryable();

         List<T> List(Expression<Func<T, bool>> where);

         int Insert(T obj);

         int Update(T obj);

         int Delete(T obj);

         int Save();

         T Find(Expression<Func<T, bool>> where);

    }




}

