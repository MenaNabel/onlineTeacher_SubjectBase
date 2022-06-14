using OnlineTeacher.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Home
{
    public class HonerListItemViewModel
    {
       
        public int StudentID { get; set; }
        public string studentName { get; set; }
        public string StudentPicture { get; set; }
        public LevelViewModel Level { get; set; }
        public double TotalExamsDegree { get; set; }
    }
}
