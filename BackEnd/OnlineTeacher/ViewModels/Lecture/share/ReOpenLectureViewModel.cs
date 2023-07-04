using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Lecture.share
{
    public class ReOpenLectureViewModel
    {
        [Required]
        public int LectureID { get; set; }
        [Required]
        public string Reason { get; set; }
    }
}
