using Microsoft.AspNetCore.Http;
using OnlineTeacher.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace OnlineTeacher.ViewModels.Lecture
{
    public class AddStudyLectureViewModel: AddLectureViewModel,IFileImage
    {
    
    
      
        [Range(1, 12, ErrorMessage ="Month Betweeen 1 and 12 ")]
        public int Month { get; set; }
        public bool IsAppear { get; set; }
        public bool IsFree { get; set; }
        
        public IFormFile ImageOrFile { get; set; }
    }
}