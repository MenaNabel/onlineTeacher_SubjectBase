using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Services.Lectures.Refactoring;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Lecture.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineTeacher.Controllers.Student
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private ISudentLectureService _lectureService;
        private ILectureServicesForStudent _lecturesServicesForStudent;
        public LecturesController(ISudentLectureService lectureService, ILectureServicesForStudent servicesForStudent)
        {
            _lectureService = lectureService;
            _lecturesServicesForStudent = servicesForStudent;
        }
        [HttpGet("MonthsForSubject/{SubjectID}")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Get(int SubjectID)
        {

            var Lectures = await _lectureService.GetLecturesForSubject(SubjectID);
            if (Lectures is not null)
                return Ok(Lectures.GroupBy(le=>le.Month).Select(s=>s.Key));

            return NotFound();
        }
        [HttpGet("Free")]
        public async Task<IActionResult> GetFree()
        {

            var Lectures = await _lectureService.GetLecturesFree();
            if (Lectures is not null)
                return Ok(Lectures);

            return BadRequest(Lectures);
        }
       // [Authorize(Roles.Student)]
        [HttpGet("{LectureID}")]
        public async Task<IActionResult> GetLecture(int LectureID)
        {
          var lec = await _lecturesServicesForStudent.GetStudingLecture(LectureID);
            if (lec is null) return NotFound();
            return Ok(lec);
            
        }
        
        [HttpGet("free/{LectureID}")]
        public async Task<IActionResult> GetFreeLecture(int LectureID)
        {
            var lec = await _lecturesServicesForStudent.GetFreeStudingLecture(LectureID);
            if (lec is null) return NotFound();
            return Ok(lec);

        }
        [Authorize(Roles.Student)]  
        [HttpGet("/Student/lectures/{SubjectID}")]
        public async Task<IActionResult> GetLectures(int SubjectID , int Month)
        {
            var lec = await _lecturesServicesForStudent.GetLecturesbyMonth(SubjectID,Month);
            if (lec is null) return NotFound();
            return Ok(lec);
        }

    }
}
