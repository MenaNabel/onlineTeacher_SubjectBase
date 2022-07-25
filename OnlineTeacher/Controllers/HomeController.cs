using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Home;
using OnlineTeacher.Services.Home.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineTeacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private ISiteInfo _siteInfo;
        private IHonerList _HonerList;
        public HomeController(ISiteInfo siteInfo , 
            IHonerList honerList)
        {
            _siteInfo = siteInfo;
            _HonerList = honerList;
        }
        [HttpGet]
        public async Task<IActionResult> Get() {
            return Ok( await _siteInfo.getSiteInfo());
        }   
        [HttpGet("Honers")]
        public async Task<IActionResult> GetHoners() {
            return Ok( await _HonerList.GetHonerList());
        }

        //[HttpGet("/VisitorIp")]
        //public  IActionResult GetVisitorIp()
        //{
        //    IPHostEntry ipHostinfo = Dns.GetHostEntry(Dns.GetHostName());
        //    string IpAddress = Convert.ToString(ipHostinfo.AddressList.FirstOrDefault(address => address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork));

        //    return Ok(IpAddress);
        //}
            

    }
}
