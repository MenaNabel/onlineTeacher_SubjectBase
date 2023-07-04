using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Lecture.share
{
    public class ReOpenLectureDetailsViewModel
    {
        [Required]
        public int LectureID { get; set; }
        [Required]
        public int StudentID { get; set; }
        [Required]

        public string LectureName { get; set; }
        public string StudentName { get; set; }
        public bool IsConfirmed { get; set; }
        public string Reason { get; set; }
    }
}
