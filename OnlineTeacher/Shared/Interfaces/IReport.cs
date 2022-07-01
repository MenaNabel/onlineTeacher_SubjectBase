using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface IReport
    {
        byte[] ExporttoExcel<T>(List<T> table, string filename);
    }
}
