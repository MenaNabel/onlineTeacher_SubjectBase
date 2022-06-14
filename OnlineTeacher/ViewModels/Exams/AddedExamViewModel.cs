using OnlineTeacher.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Exams
{
    public class AddedExamViewModel
    {
    
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Display(Name="Total Degree")]
        [Range(0, double.MaxValue)]
        public double Degree { get; set; }
        [Range(0, double.MaxValue)]
        public double Duration { get; set; }
        [Range(1, double.MaxValue)]
        public int LectureID { get; set; }
        public ExamType EaxamType { get; set; }
        public DateTime DateAndExamminationExpireTime { get; set; }
      
    }
}
