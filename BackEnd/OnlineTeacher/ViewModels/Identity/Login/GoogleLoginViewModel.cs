using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Identity.Login
{
    public class GoogleLoginViewModel
    {
        public string GoogleId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}
