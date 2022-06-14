using Microsoft.AspNetCore.Http;
using OnlineTeacher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.ViewModels.Students
{
    public class UpdatedStudnetViewModel : AddedStudentViewModel ,  IFileImage
    {
    
        public IFormFile ImageOrFile {get; set;}
    }
}
