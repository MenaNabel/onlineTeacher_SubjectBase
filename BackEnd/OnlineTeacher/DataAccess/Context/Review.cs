using Microsoft.AspNetCore.Identity;
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
    public class Review
    {
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "رائيك يهمنا من فضلك ادخله")]
        public string Description { get; set; }
       
        public bool IsAppear { get; set; } = false;
        [Required]
        [Column(TypeName = "nvarchar(450)")]
        [ForeignKey(nameof(User))]
        public string StudentID { get; set; }
        public string  StudentName { get; set; }
        public ApplicationUser  User { get; set; }

    }
   
}
