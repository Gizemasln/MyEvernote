using FakeData;
using Microsoft.Win32;
using MyEvernote.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace MyEvernote.DataAccessLayer.EntityFramework
{
//CreateDatabaseIfNotExists data base yok ise çalışsın
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {
    //Seed database oluştuktan sonra örnek veri basarken kullanılan metod
        protected override void Seed(DatabaseContext context)
        {
        //kullanıcı ekleme
            EvernoteUsers admin = new EvernoteUsers()
            {
              Name ="Gizem",
               Surname ="Aslan",
               Email ="gzmasln03@gmail.com",
                ActivateGuid = Guid.NewGuid(),
               IsActive = true,
                    IsAdmin=true,
                    Username="gizemaslan",
                    ProfileImageFilename="user.png",
                    Password="123321",
                    CreatedOn = DateTime.Now.AddMinutes(1),
                    ModifiedOn=DateTime.Now.AddMinutes(65),
                    ModifiedUsername="gizemaslan"

            };

            //admin  standart kullanıcı eklemesi için
            EvernoteUsers standartUser = new EvernoteUsers()
            {
                Name = "Ayşe",
                Surname = "Bulut",
               Email = "gzmasln03@gmail.com",
                ActivateGuid = Guid.NewGuid(),
               IsActive = true,
                    IsAdmin = false,
                    Username = "gizemaslan",
                    Password = "123321",
                ProfileImageFilename = "user.png",
                CreatedOn = DateTime.Now.AddMinutes(1),
                    ModifiedOn = DateTime.Now.AddMinutes(65),
                    ModifiedUsername = "gizemaslan"

            };
            context.EvernoteUsers.Add(admin);
            context.EvernoteUsers.Add(standartUser);

            for (int i = 0; i < 8; i++)
            {
                EvernoteUsers user = new EvernoteUsers()
                {
                    Name = FakeData.NameData.GetFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfileImageFilename = "user.png",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"

                };

                context.EvernoteUsers.Add(user);

            }
            //ekleyip kaydetmek için
            context.SaveChanges();
            //Kullanılacak kullanıcı listesi

            List<EvernoteUsers> userlist = context.EvernoteUsers.ToList();
            //FAKE CATEGORİES eklemek için
            for (int i = 0; i < 10; i++)
            { 
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description = FakeData.PlaceData.GetAddress(),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUsername = "gizemaslan"

                };
                context.Categories.Add(cat);
                // adding fake notes...

                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                {
                    EvernoteUsers owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                    Note note = new Note()
                    {

                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),//Kelime
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),//paragraf
                
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = userlist[FakeData.NumberData.GetNumber(0,userlist.Count - 1)],
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername = owner.Username,
                    };
                       cat.Notes.Add(note);

                    //Adding fake commments
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)
                    {
                        EvernoteUsers comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];
                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),         
                            Owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)],
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username,
                        };
                        note.Comments.Add(comment);
                    }
                    //Adding Fake Likes
             
                    for (int m = 0; m < note.LikeCount; m++)
                    // notun like sayısı (LikeCount) kadar dönmemeiz gerekiyor
                    {
                        Liked liked = new Liked()
                        {
                            //Like eden kullanıcı bilgisi için
                            LikedUser = userlist[m],
                        };

                        note.Likes.Add(liked);
                    }
                }
            }
            context.SaveChanges(); //oluşan tüm data yığınının  kaydet
        }

    }
}
