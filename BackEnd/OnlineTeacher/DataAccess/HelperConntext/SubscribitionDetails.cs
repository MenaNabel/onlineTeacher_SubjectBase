﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.HelperConntext
{
    public class SubscribitionDetails
    {
        public int StudentID { get; set; }
        public string Phone { get; set; }
        public int SubjectID { get; set; }
        public string StudentName { get; set; }
        public string SubjectName { get; set; }
        public bool IsActive { get; set; }
        public int LevelID { get; set; }
        public DateTime Date { get; internal set; }
    }
}
