using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OnlineTeacher.Services.Questions.Helper;
using OnlineTeacher.Shared.Exceptions;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Questions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles.Admin)]
    public class QuestionsController : ControllerBase
    {

        private readonly ILogger<QuestionsController> _logger;
        private readonly IQuestionAsync _Questions;
        public QuestionsController(ILogger<QuestionsController> logger, IQuestionAsync Questions)
        {
            _logger = logger;
            _Questions = Questions;
        }

        // Action To retrive one Question

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _Questions.Get(id));
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] AddedQuestionViewModel addedQuestion)
        {



            if (ModelState.IsValid)
            {
                if (addedQuestion.FormFile != null)
                {
                    string[] _validExtensions = { "jpg", "png", "jpeg" };
                    if (!(addedQuestion.FormFile.Length > 0
                        && (_validExtensions.Contains(addedQuestion.FormFile.FileName.Substring(addedQuestion.FormFile.FileName.Length - 3, 3).ToLower())
                        || _validExtensions.Contains(addedQuestion.FormFile.FileName.Substring(addedQuestion.FormFile.FileName.Length - 4, 4).ToLower()))))
                    {
                        return BadRequest(new UserMangerResonse("تاكد من رفع الصورة بشكل صحيح", false));
                    }
                }
                try
                {
                    return await _Questions.Add(addedQuestion) is null ? BadRequest(addedQuestion) : Ok("تم الاضافه بنجاح");
                }
                catch (EntityNotFound NotFoundExc)
                {

                    return NotFound("الامتحان غير موجود ");
                }
            }
            return BadRequest(ModelState);

        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromForm] AddedQuestionViewModel QuestionViewModel)
        {
            if (id != QuestionViewModel.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للسؤال المراد  التعديل عليه  ");
            string[] _validExtensions = { "jpg", "png", "jpeg" };
            if (QuestionViewModel.FormFile != null)
            {
                if (!(QuestionViewModel.FormFile.Length > 0
                      && (_validExtensions.Contains(QuestionViewModel.FormFile.FileName.Substring(QuestionViewModel.FormFile.FileName.Length - 3, 3).ToLower())
                       || _validExtensions.Contains(QuestionViewModel.FormFile.FileName.Substring(QuestionViewModel.FormFile.FileName.Length - 4, 4).ToLower()))))
                {
                    return BadRequest(new UserMangerResonse("تاكد من رفع الصورة بشكل صحيح", false));
                }
            }

            if (ModelState.IsValid)
            {
                var Status = await _Questions.Update(QuestionViewModel);
                return Status == HttpStatusCode.OK ? Ok("تم التعديل بنجاح ") : NotFound();
            }
            return BadRequest(QuestionViewModel);

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _Questions.Delete(id);

            return IsDeleted is true ? Ok("تم الحذف  بنجاح ") : BadRequest();
        }
    }
}
