using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subscribtions
{
    public class AddSubscibtionViewModel
    {
        [Required]
        public int StudentID { get; set; }
        [Required]
        public int SubjectID { get; set; }
    }
}
