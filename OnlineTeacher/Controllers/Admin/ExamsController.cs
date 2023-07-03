using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineTeacher.Services.Exams.Helper;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Exams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class ExamsController : ControllerBase
    {
        private readonly ILogger<ExamsController> _logger;
        private readonly IExamAsync _Exams;
        public ExamsController(ILogger<ExamsController> logger,IExamAsync Exams)
        {
            _logger = logger;
            _Exams = Exams;
        }

        [HttpGet]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Get(int index = 0, int size = 20)
        {
           return  Ok(await _Exams.GetAll(index ,size));
        }
        [HttpGet("GetExamsForStudents")]
        //Authorize(Roles.Admin)]
        public async Task<IActionResult> GetExamsForStudents(int index = 0, int size = 20)
        {
            return Ok(await _Exams.GetExamsForStudents(index, size));
        }
        [HttpGet("{id}")]
        [Authorize(Roles.Admin+","+ Roles.Student)]
        public async Task<IActionResult> Get(int id)
        {
            var Exam = await _Exams.Get(id); 
            return Exam is null ? NotFound("لا يوجد امتحان موافق ") : Ok(Exam);
        }

        [HttpGet("Search")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Search([FromQuery]string ExamName , int index =0 , int size = 10)
        {
            var Exam = await _Exams.Filter(ex => ex.Name.Contains(ExamName) , index, size);
            return Ok(Exam);
        }

      
        [HttpPost]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] AddedExamViewModel Exam)
        {
            if (ModelState.IsValid)
            {

                Exam = await _Exams.Add(Exam);
              return Exam is null ? BadRequest(Exam)  : Ok(Exam);
            }
            return BadRequest(ModelState);
        }

        [HttpPost("Accept Re open Exam")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> AcceptReOpenExamRequests([FromBody] ReopenExamFeedback reOpenExam)
        {
            if (ModelState.IsValid)
            {

                var Result = await _Exams.AcceptReOpenExam(reOpenExam);
                return Result is true ? Ok("تم الرد بنجاح") :  BadRequest(reOpenExam) ;
            }
            return BadRequest(ModelState);
        }

        [HttpGet("Re open Exam Requests")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> GetReOpenExamRequests()
        {
            
                var Result = await _Exams.GetReOpenExamRequests();
                return Result is not null ? Ok(Result) : BadRequest(Result);
         
        }


        [HttpPut("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Put(int id, [FromBody] AddedExamViewModel ExamViewModel)
        {
            if (id != ExamViewModel.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للامتحان المراد  التعديل عليها  ");
            if (ModelState.IsValid)
            {
                var Status = await _Exams.Update(ExamViewModel);
                return Status == HttpStatusCode.OK ? Ok("تم التعديل بنجاح ") : NotFound();
            }
            return BadRequest(ExamViewModel);

        }


        [HttpDelete("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _Exams.Delete(id);

            return IsDeleted is true ? Ok("تم الحذف  بنجاح ") : BadRequest();
        }
    }
}
