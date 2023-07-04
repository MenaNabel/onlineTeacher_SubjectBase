using OnlineTeacher.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineTeacher.ViewModels.Lecture
{
    public class AddLectureViewModel
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "لابد من ادخال اسم المحاضرة ")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لابد من تحديد المادة ")]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "لابد من ادخال لينك المحاضرة ")]
        [Url]
        public string LectureLink { get; set; }
        public string Description { get; set; }
        public int? LectureID { get; set; }

    }
}