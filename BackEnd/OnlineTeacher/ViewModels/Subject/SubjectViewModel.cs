using OnlineTeacher.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subject
{
    public class SubjectViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImagePath { get; set; }
        public LevelViewModel Level { get; set; }


    }
}
