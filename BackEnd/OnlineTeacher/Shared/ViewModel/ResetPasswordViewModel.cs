using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.ViewModel
{
    public class ResetPasswordViewModel
    {
        public string Email { get;  set; }
        public string NewPassword { get;  set; }
        public string ConfirmPassword { get;  set; }
        public string Token { get;  set; }
    }
}
