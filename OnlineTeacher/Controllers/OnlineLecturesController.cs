using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Lectures.Helper;
using OnlineTeacher.Services.Lectures.Refactoring;
using OnlineTeacher.Shared.Enums;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Lecture;

using System.Threading.Tasks;



namespace OnlineTeacher.Controllers
{
    //[EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    
    public class OnlineLecturesController : ControllerBase
    {
        
        private readonly ILectureServices _Lec;
        private LectureType type = LectureType.online;
        public OnlineLecturesController(ILectureServices lec)
        { 
            _Lec = lec;
        }

        [HttpGet]
        [Authorize(Roles.Admin)]
        public  IActionResult Get(int index =0, int size = 20)
        {
            return Ok(_Lec.GetAll(type , index , size));
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var Lecture = await _Lec.GetAsync(id , type);
            return Lecture is not null ? Ok(Lecture) : NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddOnlineLectureViewModel lectureViewModel)
        {
            if (ModelState.IsValid)
            {
                var Response = await _Lec.Add(lectureViewModel, type);
                return Response.IsSuccess == true ? Ok(Response) : BadRequest(Response);
            }
            ModelState.AddModelError("", "البيانات غير صحيحه ");
            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] AddOnlineLectureViewModel lectureViewModel)
        {
            if (id != lectureViewModel.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للماده المراد  التعديل عليها  ");
            if (ModelState.IsValid)
            {
                var Status = await _Lec.Update(lectureViewModel , type);
                return Status.IsSuccess is true? Ok(Status) : NotFound(Status);
            }
            return BadRequest(ModelState);
        }

       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _Lec.Delete(id);

            return IsDeleted is true ? Ok("تم الحذف  بنجاح ") : BadRequest();
        }
    }
}
