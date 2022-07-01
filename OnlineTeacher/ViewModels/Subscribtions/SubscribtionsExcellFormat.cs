using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subscribtions
{
    public class SubscribtionsExcellFormat
    {
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public bool IsActive { get; set; }
        public string  Level { get; set; }
        public DateTime Date { get; internal set; }
    }
}
