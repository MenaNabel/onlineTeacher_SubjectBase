using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Subscribtions
{
    public class UpdateSubscribtionViewModel : AddSubscibtionViewModel
    {
        [Required]
        public bool IsActive { get; set; }
    }
}
