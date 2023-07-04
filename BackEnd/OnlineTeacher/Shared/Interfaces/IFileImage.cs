using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface IFileImage
    {
        public IFormFile ImageOrFile { get; set; }
    }
}
