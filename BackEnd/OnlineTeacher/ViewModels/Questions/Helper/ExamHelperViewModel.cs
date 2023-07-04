using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Questions.Helper
{
    public class ExamHelperViewModel
    {
        public ExamHelperViewModel(int id , string name)
        {
            this.ID = id;
            this.Name = name;

        }
        public ExamHelperViewModel()
        {

        }
        public int ID { get; set; }
       
        public string Name { get; set; }
    }
}
