using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    public class HonerList
    {
        [Key]
        public int ID { get; set; }
       
        public int StudentID { get; set; }
        public string studentName { get; set; }
        public string StudentPicture { get; set; }
        [ForeignKey(nameof(Level))]
        public int LevelID { get; set; }
        
        public double TotalExamsDegree { get; set; }

        public Level Level { get; set; }


    }
}
