using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.ViewModel
{
    public class UserMangerResonse
    {
       
        public UserMangerResonse(string Message  , bool IsSuccess  , DateTime? ExpireDate , string[] Errors = default)
        {
            this.Message = Message;
            this.IsSuccess = IsSuccess;
            this.ExpireDate = ExpireDate;
            this.Errors = Errors;
        }
        public UserMangerResonse(string Message  , bool IsSuccess ,DateTime ExpireDate) : this(Message, IsSuccess, ExpireDate ,default)
        {
   
        }
        public UserMangerResonse(string Message, bool IsSuccess) : this(Message, IsSuccess, null )
        {
        
        }
        public UserMangerResonse()
        {

        }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
