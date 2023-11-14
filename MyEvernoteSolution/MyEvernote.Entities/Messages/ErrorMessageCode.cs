using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.Entities.Messages
{
    public enum ErrorMessageCode

    {
        UsernameAlredyExists = 101,
        EmailAlredyExists = 102,
        UserIsNotActive =151,
        UsernameOrPassWrong = 152,
        CheckYourEmail=153,
         UserAlredyActive=154 ,  
            ActivateIdDoesNotExist=155,
            UserNotFound=156,
        ProfileCouldNotUpdate = 157,
        UserCouldRemove = 158,
        UserCouldNotFind = 159,
        UserCouldNotInserted = 160,
        UserCouldNotUpdate = 161
    }
}
