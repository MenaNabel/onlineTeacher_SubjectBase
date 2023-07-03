using OnlineTeacher.Shared.Validation;
using System.ComponentModel.DataAnnotations;

namespace OnlineTeacher.ViewModels.Students
{
    public class updateStudentPhoneViewModel
    {
        [Required]
        public int ID { get; set; }

        [Required(ErrorMessage = "يجب ادخال رقم التليفون")]
        [Phone(ErrorMessage = "تاكد من رقم التليفون")]
        [MinLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اقل من 11 رقم"), MaxLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اكبر من 11 رقم")]

        [RegularExpression("^01(0|1|2|5)\\d{8}$", ErrorMessage = "يجب ان يبدأ الرقم ب011 او 012 او015 او  010")]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "يجب ادخال رقم التليفون")]
        [Phone(ErrorMessage = "تاكد من رقم التليفون")]
        [MinLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اقل من 11 رقم"), MaxLength(11, ErrorMessage = "لا يمكن ان يكون الرقم اكبر من 11 رقم")]

        [RegularExpression("^01(0|1|2|5)\\d{8}$", ErrorMessage = "يجب ان يبدأ الرقم ب011 او 012 او015 او  010")]
        [NotSqlInjecttion(ErrorMessage = "Sql Injection Error")]
        public string ParentPhone { get; set; }

    }
}
