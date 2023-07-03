using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    
    public class Student
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }

       
        [Phone]
        public string Phone {get;set;} 
        [Phone]
        public string ParentPhone { get;set;}
        [Required]
        public string City { get; set; }
        [Required]
      
        public string Email { get; set; }
        [Required]
        public  string school { get; set; }

        [ForeignKey(nameof(Level))]
        public int LevelID { get; set; }
       
        public string Image { get; set; }

        // Bridges
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; }
        public Level Level { get; set; }
        public ICollection<StudentExam> StucentExam { get; set; }
        public Student()
        {
            Reviews = new HashSet<Review>();
            Subscriptions = new HashSet<Subscription>();
         
            StucentExam = new HashSet<StudentExam>();
            
         
        }
       
    }
}
