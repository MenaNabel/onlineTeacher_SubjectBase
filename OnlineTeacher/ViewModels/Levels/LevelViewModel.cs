using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Levels
{
    public class LevelViewModel
    {
  
        public int ID { get; set; }
        [Required]
        public string LevelName { get; set; }
        [Required(ErrorMessage = "لابد من  ادخال الرابط")]
        [Url]
        public string TelegeramLink { get; set; }
    }
}
