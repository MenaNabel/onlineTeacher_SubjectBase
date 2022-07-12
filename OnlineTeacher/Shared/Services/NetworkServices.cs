using OnlineTeacher.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Shared.Services
{
    public class NetworkServices : INetwork
    {
        public string GetVisitorIp()
        {
            IPHostEntry ipHostinfo = Dns.GetHostEntry(Dns.GetHostName());
            string IpAddress = Convert.ToString(ipHostinfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            return IpAddress;
        }
    }
}
