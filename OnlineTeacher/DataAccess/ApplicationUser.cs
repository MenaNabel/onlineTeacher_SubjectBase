using Microsoft.AspNetCore.Identity;


namespace OnlineTeacher.DataAccess
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string VisitorIP { get; set; }
        public string VisitorIP2 { get; set; }
        public int VisitorIpsAssignedNo { get; set; }


        public bool IsAssignedIp(string ip)
        {
            if (VisitorIpsAssignedNo == 2)
            {
                if (ip == VisitorIP || ip == VisitorIP2)
                    return true;
                return false;
            }
            else if (VisitorIpsAssignedNo == 1)
            {
                if (ip == VisitorIP)
                    return true;
                return false;
            }
            return false;
        }
        public bool Assign(string ip)
        {
            if (IsAssignedIp(ip))
                return true;
            if (VisitorIpsAssignedNo >= 2)
                return false;
            if (VisitorIpsAssignedNo == 1)
            {
                VisitorIP2 = ip;
                VisitorIpsAssignedNo++;
            }
            if (VisitorIpsAssignedNo == 0)
            {
                VisitorIP = ip;
                VisitorIpsAssignedNo++;
            }
            return true;
        }
        public bool DeleteIp(string ip)
        {
            if (!IsAssignedIp(ip))
                return false;
            if (VisitorIP == ip)
            {
                VisitorIP = default;
                if (VisitorIpsAssignedNo > 1)
                {
                    VisitorIP = VisitorIP2;
                }
                VisitorIpsAssignedNo--;
                return true;
            }
            if (VisitorIP2 == ip)
            {
                VisitorIP2 = default;
                VisitorIpsAssignedNo--;
                return true;
            }
            return false;

        }
    }
}