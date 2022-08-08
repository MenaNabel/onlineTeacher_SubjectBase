using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineTeacher.Services.Exams.Helper;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineTeacher.Controllers.Student
{
    [Route("Student/[controller]")]
    [ApiController]
    public class ExamsController : ControllerBase
    {

        private readonly ILogger<ExamsController> _logger;
        private readonly IExamingServicesAsync _Exams;
        public ExamsController(ILogger<ExamsController> logger, IExamingServicesAsync Exams)
        {
            _logger = logger;
            _Exams = Exams;
        }
        [HttpGet("{id}")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Get(int id)
        {
            var Exam = await _Exams.GetExamForCurrentStudent(id);
            return Exam is null ? NotFound("لا يوجد امتحان موافق ") : Ok(Exam);
        }
        [HttpGet("Grades/{id}")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Grades(int id)
        {
            var Exam = await _Exams.GradesProgress(id);
            return Exam is null ? NotFound("لا يوجد امتحانات حتي الان") : Ok(Exam);
        }
        [HttpPost]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Post(ExamCorrectionViewModel exam)
        {
            var Exam = await _Exams.ExamCorrection(exam);
            return Exam is null ? NotFound("يوجد خطأ ") : Ok("تم التصحيح بنجاح");
        }
        [HttpPost("ReExam")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> ReExam(ExamCorrectionViewModel exam)
        {
            var Exam = await _Exams.Re_ExamCorrection(exam);
            return Exam is null ? BadRequest("يوجد خطأ ") : Ok("تم التصحيح بنجاح");
        }
        [HttpPost("Re open Exam")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> ReOpenExam(ReOpenExamViewModel exam)
        {
            var Exam = await _Exams.ReOpenExam(exam);
            return Exam is true ? Ok("تم الطلب بنجاح") : BadRequest("يوجد مشكله في اعاده فتح الامتحان");
        }
        [HttpGet("MyExams")]
        public async Task<IActionResult> GetMyExams()
        {
            return Ok(await _Exams.GetExamsForCurrentStudent());
        }
    }
}
