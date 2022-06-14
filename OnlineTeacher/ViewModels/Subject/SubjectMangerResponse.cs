using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subject
{
    public class SubjectMangerResponse
    {

        public SubjectMangerResponse(string message ,bool IsSuccess,List<string> Errors ) 
        {
            this.message = message;
            this.IsSuccess = IsSuccess;
            this.Errors = Errors;
        
        }
        public SubjectMangerResponse(string message ,bool IsSuccess ) :
            this(message , IsSuccess ,default)
        { 
        
        }
        public SubjectMangerResponse()
        {
            Errors = new List<string>();
        }
        public string message { get; private set; }
        public bool IsSuccess { get; private set; }
        public List<string> Errors { get; set; }
    }
}
