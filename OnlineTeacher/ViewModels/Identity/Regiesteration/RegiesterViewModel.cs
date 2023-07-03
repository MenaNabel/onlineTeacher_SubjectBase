using OnlineTeacher.Shared.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Identity.Regiesteration
{
    public class RegiesterViewModel
    {
        [Required(ErrorMessage = "يجب ادخال الاسم")]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string Name { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم التليفون")]
        [Phone(ErrorMessage ="تاكد من رقم التليفون")]
        [MinLength(11 ,ErrorMessage ="لا يمكن ان يكون الرقم اقل من 11 رقم") ,MaxLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اكبر من 11 رقم")]

        [RegularExpression("^01(0|1|2|5)\\d{8}$" ,ErrorMessage = "يجب ان يبدأ الرقم ب011 او 012 او015 او  010")]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم التليفون")]
        [Phone(ErrorMessage = "تاكد من رقم التليفون")]
        [MinLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اقل من 11 رقم"), MaxLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اكبر من 11 رقم")]

        [RegularExpression("^01(0|1|2|5)\\d{8}$", ErrorMessage = "يجب ان يبدأ الرقم ب011 او 012 او015 او  010")]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string ParentPhone { get; set; }

        [Range( minimum:1 , maximum:double.MaxValue, ErrorMessage ="تأكد من اختيار المرحلة")]
        public int LevelID { get; set; }

        [Required(ErrorMessage ="يجب ادخال البريد الالكترونى")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "يجب ان يكون البريد الالكترونى اقل من 50 حرف")]

        [EmailAddress]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string Email { get; set; }

        
        [Required(ErrorMessage = "تأكد من ادخال كلمة المرور")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "يجب ان تكون كلمة المرور اكثر من 5 حروف واقل من 50 حرف")]
        [DataType(DataType.Password)]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string Password { get; set; }

        [Required(ErrorMessage ="تأكد من ادخال تاكيد كلمة المرور")]
        [Compare(nameof(Password), ErrorMessage ="تأكد من ادخال تأكيد كلمة المرور مشابهة لكلمة المرور")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "يجب ان تكون كلمة المرور اكثر من 5 حروف واقل من 50 حرف")]

        [DataType(DataType.Password)]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string ConfirmPassword { get; set; }
        public string SchoolName { get; set; }
        public string city { get; set; }
    }
}
