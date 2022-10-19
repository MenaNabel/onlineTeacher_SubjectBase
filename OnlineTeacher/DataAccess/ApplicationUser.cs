using Microsoft.AspNetCore.Identity;
using OnlineTeacher.Shared.ViewModel;

namespace OnlineTeacher.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string VisitorIP { get; set; }
        public string MacAddress { get; set; }
        public string MacAddress2 { get; set; }
        public string VisitorIP2 { get; set; }
        public int VisitorIpsAssignedNo { get; set; }


        public bool IsAssignedIp(NetworkViewMode netorkInfo)
        {
            if (VisitorIpsAssignedNo == 2)
            {
               // if (netorkInfo.IP == VisitorIP || netorkInfo.IP == VisitorIP2 || netorkInfo.MacAddress == MacAddress || netorkInfo.MacAddress == MacAddress2)
                if (netorkInfo.MacAddress == MacAddress || netorkInfo.MacAddress == MacAddress2)
                    return true;
                return false;
            }
            else if (VisitorIpsAssignedNo == 1 )
            {
               // if (netorkInfo.IP == VisitorIP || netorkInfo.MacAddress == MacAddress)
                if (netorkInfo.MacAddress == MacAddress)
                    return true;
                return false;
            }
            return false;
        }
        public bool Assign(NetworkViewMode netorkInfo)
        {
            if (IsAssignedIp(netorkInfo))
                return true;
            if (VisitorIpsAssignedNo >= 2)
                return false;
            if (VisitorIpsAssignedNo == 1)
            {
                VisitorIP2 = netorkInfo.IP;
                MacAddress2 = netorkInfo.MacAddress;
                VisitorIpsAssignedNo++;
            }
            if (VisitorIpsAssignedNo == 0)
            {
                VisitorIP = netorkInfo.IP;
                MacAddress = netorkInfo.MacAddress;
                VisitorIpsAssignedNo++;
            }
            return true;
        }
        public bool DeleteIp()
        {
            VisitorIP = default;
            VisitorIP2 = default;
            MacAddress =default;
            MacAddress2 =default;
            return false;

        }
    }
}