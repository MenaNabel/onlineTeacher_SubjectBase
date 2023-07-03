using Microsoft.AspNetCore.Identity;
using OnlineTeacher.DataAccess.Context;
using OnlineTeacher.Shared.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineTeacher.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        [ForeignKey(nameof(student))]
        public int? studentID { get; set; }
        
        public Student student{ get; set; }


    }      
}