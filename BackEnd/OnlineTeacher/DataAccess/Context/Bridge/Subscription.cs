using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    [Index(nameof(StudentID))]
    public class Subscription
    {
        [Required]
        [Key]
        [Column(Order = 1 )]
        [ForeignKey(nameof(Subject))]
        public int SubjectID { get; set; }
        [Required]
        [Key]
        [Column(Order = 2)]
        [ForeignKey(nameof(Student))]
        public int StudentID { get; set; }
        public bool IsActive { get; set; }
        public DateTime DataAndTime { get; set; } = DateTime.Now;
       
        // Bridges
        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
