using System;
using System.Collections.Generic;
using System.Configuration;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernoteCommon.Helpers
{
    public class ConfigHelper
    {

        public static T Get<T>(string key)
        {
            return (T)Convert.ChangeType(ConfigurationManager.AppSettings[key], typeof(T));

            //Convert,ChangeType =önce dönüştürecek olduğum değeri ver daha sonra dönüştüreceğüö tipi ver
            //T .Generic hangi veri tipi ile giriş yaparsan o veri tipini döndürürüm

        }

    }
}
