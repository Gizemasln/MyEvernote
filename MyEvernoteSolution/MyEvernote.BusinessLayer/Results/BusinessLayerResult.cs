using MyEvernote.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEvernote.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T : class
    {

        //public List<string> Errors { get; set; } hata mesajının kod tanımlanmamış hali 

        //ErrorMessageCode classı ekledikten sonra bu kısmı düzenliyorum
        //KeyValuePair:iki tane tip alabilir hale getirmek için
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }
        public BusinessLayerResult()
        {

        // Errors = new List<string>();
        Errors = new List<ErrorMessageObj>();
    }
        public void AddError(ErrorMessageCode code, string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code, Message = message });
                
             

        }


    }
}
