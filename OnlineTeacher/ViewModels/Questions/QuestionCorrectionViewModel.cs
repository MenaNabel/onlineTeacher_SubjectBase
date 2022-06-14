using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Questions
{
    public class QuestionCorrectionViewModel
    {
        public int ID { get; set; }
        public int CorrectAnswer { get; set; }
        public int ActualAnswer  { get; set; }
        
    }
    public class QuestionDegree
    {
        public int ID { get; set; }
      
        public double Degree { get; set; }

    }
}
