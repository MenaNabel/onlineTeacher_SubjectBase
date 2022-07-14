using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Subscriptions.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Subscribtions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class SubscribtionController : ControllerBase
    {
        private readonly ISubscribtion _Subscribtion;
        private readonly IReport _IReport;

        public SubscribtionController(ISubscribtion Subscribtion, IReport IReport)
        {
            _Subscribtion = Subscribtion;
            _IReport = IReport;

        }

        [HttpGet]
        [Authorize(Roles.Admin)]
        public IActionResult GetSubscribtions(int index  =0, int size=20)
        {
            return Ok(_Subscribtion.GetAllSubscrbtion(index , size));
        }
        //[HttpGet("DownloadInfo")]
        //[Authorize(Roles = Roles.Admin)]
        //public  IActionResult DownloadSubscrbtion()
        //{
        //    string reportname = $"Subscriptions_{Guid.NewGuid():N}.xlsx";
        //    var Subscribtions =  _Subscribtion.GetAllSubscrbtion();

        //    if (Subscribtions.Items.ToList().Count > 0)
        //    {
        //        var exportbytes = _IReport.ExporttoExcel<SubscribtionsExcellFormat>(
        //            Subscribtions.Items.Select(sub => new SubscribtionsExcellFormat { StudentName = sub.StudentName, SubjectName = sub.SubjectName, Date = sub.Date, IsActive = sub.IsActive, Level = sub.LevelID.ToString() }).ToList()
        //            , reportname);
        //        return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
        //    }
        //    return NoContent();
        //}

        [HttpGet("Not Confirmed")]
        [Authorize(Roles.Admin)]
        public IActionResult GetNotConfirmedSubscribtions()
        {
            return Ok(_Subscribtion.GetSubscrbtionsNotAccepeted());
        }


        
        [HttpGet("my")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> GetSubscribtionsForStudent()
        {
            try
            {
                return Ok(await _Subscribtion.GetAllSubscrbtionForCurrentStudent());
            }
            catch (UnauthorizedAccessException ex) {

                return Unauthorized();
            }
            catch (Exception ex) {

                return Unauthorized();
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddSubscibtionViewModel Subscibtion)
        {
            if (ModelState.IsValid)
            {
                var Resonse = await _Subscribtion.Subscribe(Subscibtion);
                if (Resonse.IsSuccess)
                    return Ok(Resonse);
                return BadRequest(Resonse);
            }
            return BadRequest(Subscibtion);
        }


        //[HttpPut("Update")]
        //public async Task<IActionResult> Put([FromBody] UpdateSubscribtionViewModel Subscibtion)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var Result = await _Subscribtion.Active(Subscibtion);
        //        return Result.IsSuccess is true ? Ok(Result) : BadRequest(Result);
        //    }
        //    return BadRequest(Subscibtion);
        //}
        //[HttpPut("UpdateRange")]
        [HttpPut]

        public async Task<IActionResult> PutRange([FromBody] IEnumerable<UpdateSubscribtionViewModel> Subscibtion)
        {
            if (ModelState.IsValid)
            {
                var Result = await _Subscribtion.Active(Subscibtion);
                return Result.IsSuccess is true ? Ok(Result) : BadRequest(Result);
            }
            return BadRequest(Subscibtion);
        }
        //[HttpDelete("Delete")]
        //public async Task<IActionResult> Delete(AddSubscibtionViewModel subscibtionViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var response = await _Subscribtion.Remove(subscibtionViewModel);
        //        return response.IsSuccess is true ? Ok(response) : BadRequest(response);
        //    }
        //    return BadRequest(subscibtionViewModel);
        //}
        //[HttpDelete("DeleteRange")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteRange([FromBody] IEnumerable<AddSubscibtionViewModel> addSubscibtionViewModel)
        {
            if (ModelState.IsValid)
            {
                var response = await _Subscribtion.Remove(addSubscibtionViewModel);
                return response.IsSuccess is true ? Ok(response) : BadRequest(response);
            }
            return BadRequest(addSubscibtionViewModel);
        }
    }
}
