﻿using System.IO;
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
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Lecture;
using OnlineTeacher.ViewModels.Lecture.Helper;

namespace OnlineTeacher.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class StudyLecturesController : Controller
    {
        private readonly ILectureServices _lectures;
        private LectureType type = LectureType.studying;
        public StudyLecturesController(ILectureServices lectures)
        {
            _lectures = lectures;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Lectures = await _lectures.GetAll(type);
            return Ok(Lectures.Select(le=>(StudeingLectureViewModel)le));
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
    }
}