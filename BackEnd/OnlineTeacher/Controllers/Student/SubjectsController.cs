using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Subjects.Helper;
using OnlineTeacher.Shared.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace OnlineTeacher.Controllers.Student
{
    [Route("Student/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectAsync _subjects;
        public SubjectsController(ISubjectAsync subject)
        {
            _subjects = subject;
        }

        [HttpGet("me")]
        [Authorize(Roles.Student)]
        public async Task<IActionResult> Get()
        {
            return Ok(await _subjects.GetAllForCurrentStudent());
        }
    }
}
