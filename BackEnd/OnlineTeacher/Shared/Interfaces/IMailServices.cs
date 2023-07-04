using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface IMailServices
    {
        Task SendEmailAsync(string ToEmail , string Subject , string Content);
    }
}
