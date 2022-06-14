using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.DataAccess.Context
{
    public class LectureDetails
    {
        public int ID { get; set; }
        public byte[] FileData { get; set; }
    }
}
