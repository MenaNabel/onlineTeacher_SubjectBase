
using System.Web;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AspNetCoreCurrentRequestContext;

namespace OnlineTeacher.Shared.Services
{
    public class NetworkServices : INetwork
    {
        public NetworkViewMode GetVisitorIp()
        {
            IPHostEntry ipHostinfo = Dns.GetHostEntry(Dns.GetHostName());
            NetworkViewMode networkInfo = new NetworkViewMode();
            
            networkInfo.IP = Convert.ToString(ipHostinfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));
            networkInfo.MacAddress = GetMacAddress();
            

            // Work On Windows Only not in mac or linux 

            //ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //ManagementObjectCollection moc = mc.GetInstances();
            //string MACAddress = String.Empty;
            //foreach (ManagementClass mo in moc)
            //{ 
            //    if(MACAddress == String.Empty)
            //    {
            //        if ((bool)mo["IPEnabled"] == true)
            //        {
            //            MACAddress = mo["MacAddress"].ToString();

            //        }
            //        mo.Dispose();
            //    }
            //    MACAddress = MACAddress.Replace(":", "-");
            //}
            return networkInfo;
        }

        public NetworkViewMode GetVisitorIp(HttpContext context)
        {
            NetworkViewMode networkInfo = new NetworkViewMode();
            string strHostName = "";
           // strHostName = System.Net.Dns.;

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            networkInfo.MacAddress= String.Join(',' , addr.AsEnumerable())+','+strHostName;




            networkInfo.IP = context.Connection.RemoteIpAddress.MapToIPv4().ToString();
            return networkInfo;
        }

        private string GetMacAddress()
        {
            //string mac = "";
            //foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            //{

            //    if (nic.OperationalStatus == OperationalStatus.Up && (!nic.Description.Contains("Virtual") && !nic.Description.Contains("Pseudo")))
            //    {
            //        if (nic.GetPhysicalAddress().ToString() != "")
            //        {
            //            mac = nic.GetPhysicalAddress().ToString();
            //        }
            //    }
            //}
            //return mac;

            ////////////////////////////////////////////////////////////
            //string ipaddress;
            //ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (ipaddress == "" || ipaddress == null)
            //    ipaddress = Request.ServerVariables["REMOTE_ADDR"];

            //return ipaddress;
            /////////////////////////////////////////////////////////////////////
            //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //String sMacAddress = string.Empty;
            //foreach (NetworkInterface adapter in nics)
            //{
            //    //if (sMacAddress == String.Empty)// only return MAC Address from first card  
            //    //{
            //    IPInterfaceProperties properties = adapter.GetIPProperties();
            //    string macTemp = adapter.GetPhysicalAddress().ToString();
            //    if (String.IsNullOrEmpty(macTemp) || String.IsNullOrWhiteSpace(macTemp))
            //    {
            //        continue;
            //    }

            //    sMacAddress = sMacAddress+'.'+ macTemp;
            //    //}
            //}
            //return sMacAddress;
            /////////////////////////////////////////////////
            HttpContext context = AspNetCoreHttpContext.Current;
            string ipAddress = context.Request.Headers["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.Headers["REMOTE_ADDR"];
        }
    }
    //protected string GetIPAddress()
    //{
    //    HttpContext context = AspNetCoreHttpContext.Current;
    //    string ipAddress = context.Request.Headers["HTTP_X_FORWARDED_FOR"];

    //    if (!string.IsNullOrEmpty(ipAddress))
    //    {
    //        string[] addresses = ipAddress.Split(',');
    //        if (addresses.Length != 0)
    //        {
    //            return addresses[0];
    //        }
    //    }

    //    return context.Request.Headers["REMOTE_ADDR"];
    //}
}
