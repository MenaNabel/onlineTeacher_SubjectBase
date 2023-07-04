using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Reviews
{
    public class ReviewViewModel
    {
       
      
        [Required(ErrorMessage = "رائيك يهمنا من فضلك ادخله")]
        public string Description { get; set; }
        


    }
}
