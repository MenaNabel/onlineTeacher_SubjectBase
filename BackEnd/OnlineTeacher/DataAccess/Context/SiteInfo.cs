using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    public class SiteInfo
    {
        public int ID { get; set; }
        public int StudentsCount { get; set; }
        public int LecturesCount { get; set; }
        public int ExamsCount { get; set; }
        public int FilesCount { get; set; }
    }
}
