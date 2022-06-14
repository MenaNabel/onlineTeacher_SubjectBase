using OnlineTeacher.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Identity.Login
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="يجب ادخال البريد الالكترونى")]
        [StringLength(50,ErrorMessage ="البريد الاكترونى لا يتعدى 50 حرف")]
        [EmailAddress(ErrorMessage ="تاكد من ادخال البريد الاكترونى بشكل صحيح")]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string Email { get; set; }
        [Required(ErrorMessage ="يجب ادخال كلمة المرور")]
        [DataType(DataType.Password)]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string Password { get; set; }
    }
}
