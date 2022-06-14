
using Microsoft.AspNetCore.Http;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subject
{
    public class AddingSubjectViewModel : IFileImage
    {
  
        public int ID { get; set; }
        [Required(ErrorMessage ="لابد من ادخال اسم الماده ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لابد من ادخال السعر")]
        [Range(minimum: 0, maximum: double.MaxValue, ErrorMessage = "لا يمكن ان تكون قيمه الاشتراك اقل من صفر جنيه ")]
        public double Price { get; set; }
        [Required(ErrorMessage = "لابد من اختيار المستوي الدراسي")]

        public int LevelID { get; set; }
        public IFormFile ImageOrFile { get; set; }
    }
}
