using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.DataAccessLayer.EntityFramework
{
    public class DatabaseContext : DbContext 
    /*CodeFirst (kod ile veritabanı oluşturma)
    için DbContext sınıfından türemesi gerekiyor paket ile dahil ediliyor*/
    {
        //TABLOLARA KARŞILIK GELEN DB SETLER
        public DbSet<EvernoteUsers> EvernoteUsers { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Liked> Likes { get; set; }
        public DatabaseContext() { 

        //VEri tabanı başlatıcısı
        Database.SetInitializer(new MyInitializer());
        
        }

        //İLİŞKİLİ VERİ SİLME EZMESİNİ DATABASE OLUŞTURULURKEN CASCADE YAPMAK İÇİN
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //FluentAPI

            modelBuilder.Entity<Note>() //Note tablosu üzerinde işlem yapacağım
                .HasMany(n => n.Comments)//Comment tablosu ile bir e çok ilişkili
                .WithRequired(c => c.Note)//Bir commentin notu olmak zorundadır
                .WillCascadeOnDelete(true);


            modelBuilder.Entity<Note>() //Note tablosu üzerinde işlem yapacağım
         .HasMany(n => n.Likes)//Comment tablosu ile bir e çok ilişkili
         .WithRequired(c => c.Note)//Bir commentin notu olmak zorundadır
         .WillCascadeOnDelete(true);



        }


    }
    }
