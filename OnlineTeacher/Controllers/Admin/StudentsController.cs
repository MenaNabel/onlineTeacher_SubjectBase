using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Students.Helper;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        public readonly IStudentAsync _Student;
        public StudentsController(IStudentAsync student)
        {
            _Student = student;
        }
        [HttpGet()]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> GetAll(int index =0 , int size=20)
        {
            return Ok(await _Student.GetAll(index, size));
        }

        [HttpGet("me")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Get()
        {
            var Student =  await _Student.GetAsync();
            return Student is not null ? Ok(Student) : NotFound() ;
        }


        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] AddedStudentViewModel studentViewModel)
        //{
        //    if (ModelState.IsValid) {

        //     var StudentAdded = await   _Student.Add(studentViewModel);
        //        return StudentAdded is not null ? Ok("تم الاضافه بنجاح"): BadRequest() ;
        //    }
        //    return BadRequest(ModelState);
        //}
        [HttpGet("filter")]
        // [Authorize(Roles = Roles.Admin)]
        [AllowAnonymous]
        public async Task<IActionResult> filter(string studentName = "" , string phone = "")
        {
            try
            {
                return Ok(await _Student.filter(st => st.Name.Contains(studentName) && st.Phone.Contains(phone)));
            }
            catch
            {
                return NotFound();  
            }
        }
      
        [HttpPut]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Put([FromForm] UpdatedStudnetViewModel studentViewModel)
        {
         
            if (ModelState.IsValid)
            {
                var IsUpdated = await _Student.Update(studentViewModel);
                return IsUpdated is  true ? Ok("تم العديل بنجاح") : NotFound();
            }
            return BadRequest(ModelState);
        }

        //// DELETE api/<StudentsController>/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //}
    }
}
