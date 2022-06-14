using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Reviews
{
    public class ReviewDetailsViewModel
    {
        
        public int ID { get; set; }
      
        public string Description { get; set; }

        public bool IsAppear { get; set; } = false;
       
        public string StudentName { get; set; }

    }
}
