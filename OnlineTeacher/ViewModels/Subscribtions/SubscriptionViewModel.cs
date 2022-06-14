using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subscribtions
{
    public class SubscriptionViewModel
    {
        public int StudentID { get; set; }
        public int SubjectID { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public bool IsActive { get; set; }
        public int LevelID { get; set; }
        public DateTime Date { get; internal set; }
    }
}
