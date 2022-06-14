using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    [Index(nameof(LevelID))]
    public class Subject
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [Range(minimum:0 , maximum:double.MaxValue , ErrorMessage ="لا يمكن ان تكون قيمه الاشتراك اقل من صفر جنيه ")]
        public double Price { get; set; }

        [ForeignKey(nameof(level))]
        public int LevelID { get; set; }

        public Level level { get; set; }
        public string  ImagePath { get; set; }

        // Bridges

        public ICollection<Subscription> Subscriptions { get; set; }
        public ICollection<Lecture> Lectures { get; set; }
     

        public Subject()
        {
            Lectures = new HashSet<Lecture>();
            Subscriptions = new HashSet<Subscription>();
        }
    }
    
}
