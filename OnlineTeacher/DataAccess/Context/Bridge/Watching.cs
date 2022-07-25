using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context.Bridge
{
    public class Watching
    {
        [Key]
        [ForeignKey(nameof(Student))]
        [Column(Order = 1)]
        public int StudentID { get; set; }
        [Key]
        [ForeignKey(nameof(Lecture))]
        [Column(Order = 2)]
        public int LectureID { get; set; }
        public int WatchingCount { get; set; }
        public Student Student { get; set; }
        public Lecture Lecture { get; set; }


        private bool AllowToWatch()
        {
            return WatchingCount >= 5 ? false : true;
        }
        public bool Watch()
        {
            if (AllowToWatch())
            {
                WatchingCount++;
                return true;
            }
            return false;
        }
        public bool ReOpenWatching() 
        {
            if (!AllowToWatch())
            {
                WatchingCount -= 3;
                return true;
            }
            return false;
        }
    }
}
