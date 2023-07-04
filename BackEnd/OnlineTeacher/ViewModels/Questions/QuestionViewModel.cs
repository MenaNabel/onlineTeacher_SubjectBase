using Microsoft.AspNetCore.Http;
using OnlineTeacher.ViewModels.Questions.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Questions
{
    public class QuestionViewModel
    {
       
        public int ID { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string OneAnswer { get; set; }
        [Required]
        public string SecondAnswer { get; set; }
        [Required]
        public string ThirdAnswer { get; set; }
        [Required]
        public string FourthAnswer { get; set; }
        [Required]
        [Range(1, 4)]
        public int CorrectAnswer { get; set; }
        public IFormFile FormFile { get; set; }
        public string FileName { get; set; }
        public string File { get; set; }

    }
}
