﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Services.Lectures.Refactoring;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;
using OnlineTeacher.ViewModels.Lecture.share;

namespace OnlineTeacher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class StudyLecturesController : Controller
    {
        private readonly ILectureServices _lectures;
       private readonly IReport _IReport;
        private LectureType type = LectureType.studying;
        private readonly IWatcher _watcher;
        public StudyLecturesController(ILectureServices lectures, IReport IReport, IWatcher watcher)
        {
            _lectures = lectures;
             _IReport = IReport;
            _watcher = watcher;
        }
        [HttpGet]
        public async Task< IActionResult> Get(int index =0 , int size= 20)
        {
            var Lectures = await _lectures.GetAll(type , index, size);
            return Ok(Lectures);
        }

        [HttpPost("Request_Open_Watching")]
        [Authorize(Roles =Roles.Student)]
        public async Task<IActionResult> ReOpenWatchingRequest(ReOpenLectureViewModel reOpenLecture)
        {
            if ( await  _lectures.ReOpenWatchingRequest(reOpenLecture))
                return Ok("Request Successfuly");
            return BadRequest("we Have a Problem please try again");
           
        }
        [HttpPost("Confirm_OpenWatching")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> ConfirmOpenWatching(ReOpenLectureDetailsViewModel reOpenLecture)
        {
            if (await _lectures.ConfirmReOpenWatching(reOpenLecture))
                return Ok("Confirmed Successfuly");
            return BadRequest("we Have a Problem please try again");

        }
        [HttpGet("Get_ReOpenWatchingRequests")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetReOpenWatchingRequests(int index =0, int size =0)
        {
            var requests = await _lectures.GetReOpenLectureRequest(index ,size);
                return Ok(requests);
           

        }
        [HttpGet("DownloadInfo")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DownloadLectures()
        {
            string reportname = $"Lecture_{Guid.NewGuid():N}.xlsx";
            var Lectures = await _lectures.GetAll(type, 0, 10000);

            if (Lectures.Items.ToList().Count > 0)
            {
                var exportbytes = _IReport.ExporttoExcel<LectureExcelFormat>(
                    Lectures.Items.Select(lec => new LectureExcelFormat { Name = lec.Name, Description = lec.Description, LectureLink = lec.LectureLink, Type = lec.Type }).ToList()
                    , reportname);
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
            }
            return NoContent();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetByAdmin/{id}")]
        public async Task<IActionResult> GetByAdmin(int id)
        {
            var Lecture = await _lectures.GetAsyncByAdmin(id, type);
            return Lecture is not null ? Ok((StudeingLectureViewModel)Lecture) : NotFound();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetAllLectures/{id}")]
        public  IActionResult GetAllLecturesByAdmin(int id)
        {
            var Lectures =  _lectures.GetSubjectLectures(id, type);
            return Lectures is not null ? Ok(Lectures) : NotFound();
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
           
            var Lecture = await _lectures.GetAsync(id, type);
            return Lecture is not null ?Ok((StudeingLectureViewModel)Lecture) : NotFound();
        }
        [HttpGet("Files/{LectureID}")]

        public async Task<IActionResult> DownLoad(int LectureID)
        {

            var FileWithdata = await _lectures.GetFile(LectureID);
           
            return Ok(FileWithdata);
            
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm]AddStudyLectureViewModel lectureViewModel)
        {
            if (ModelState.IsValid)
            {
                var Response = await _lectures.Add(lectureViewModel, type);
                return Response.IsSuccess == true ? Ok(Response) : BadRequest(Response);
            }
            ModelState.AddModelError("", "البيانات غير صحيحه ");
            return BadRequest(ModelState);
        }

      
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AddStudyLectureViewModel lectureViewModel)
        {
            if (id != lectureViewModel.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للماده المراد  التعديل عليها  ");
            if (ModelState.IsValid) {
                var Status = await _lectures.Update(lectureViewModel, type);
                return Status.IsSuccess == true ? Ok(Status) : NotFound(Status);
            }
            return BadRequest(ModelState);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _lectures.Delete(id);

            return IsDeleted is true ? Ok("تم الحذف  بنجاح ") : BadRequest();
        }

        [HttpGet("filter")]
        [AllowAnonymous]
        public async Task<IActionResult> filter(string Name = "" ,int index =0 , int size = 10) 
        {
            return Ok(await _lectures.filter(lec => lec.Name.Contains(Name) , index , size) );
        }
    }
}
