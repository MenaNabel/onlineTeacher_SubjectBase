using Microsoft.AspNetCore.Http;
using OnlineTeacher.Shared.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Interfaces
{
    public interface INetwork
    {
        NetworkViewMode GetVisitorIp();
        NetworkViewMode GetVisitorIp(HttpContext context);
    }
}
