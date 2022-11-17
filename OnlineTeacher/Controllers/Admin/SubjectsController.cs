using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Subjects.Helper;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;


namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
   [Authorize]
    public class SubjectsController : ControllerBase
    {
        private readonly ISubjectAsync _subjects;
        private readonly IReport _IReport;
        public SubjectsController(ISubjectAsync subject , IReport IReport)
        {
            _subjects = subject;
            _IReport = IReport;
        }
        /// <summary>
        /// Retrive List Of Subjects 
        /// </summary>
        /// <returns>
        /// List Of AddingsubjctViewModel
        /// </returns>
        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Get(int index=0 , int size=20)
        {
            return Ok(await _subjects.GetAll(index, size));
        }
        [HttpGet("/all")]
       // [Authorize(Roles = Roles.Admin)]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            return Ok(await _subjects.GetAll());
        }

        [HttpGet("DownloadInfo")]
         [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> DownloadSubject()
        {
            string reportname = $"Subjects_{Guid.NewGuid():N}.xlsx";
            var Subjects = await _subjects.GetAll(0,10000);
            
            if (Subjects.Items.ToList().Count > 0)
            {
                var exportbytes = _IReport.ExporttoExcel<SubjectExcellFormat>(
                    Subjects.Items.Select(sub=> new SubjectExcellFormat { Name= sub.Name, Price = sub.Price,  ImagePath = sub.ImagePath , LevelName = sub.Level.LevelName }).ToList()
                    , reportname); 
                return File(exportbytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", reportname);
            }
            return NoContent();
        }
        [HttpGet("{id}")]
        //[Authorize(Roles = Roles.Admin+","+Roles.Student)]
        public async Task<IActionResult> Get(int id)
        {
          var Subject = await _subjects.GetAsync(id);
           return Subject is not null ?  Ok(Subject) : NotFound();
        }



        [HttpPost]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Post([FromForm] AddingSubjectViewModel subject)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _subjects.Add(subject); 
                    return response.IsSuccess ? Ok(response) : BadRequest(response);
                }
                catch (Exception ex) {
                    return BadRequest(ex.InnerException?.Message);
                
                }
              
            }
            ModelState.AddModelError("", "البيانات غير صحيحه ");
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Add subject To Data base  
        /// </summary>
        /// <param name="id"> int  </param>
        /// <param name="subjectViewModel"> Adding Subject View  Model</param>
        /// <returns>
        /// if updated return HttpStatusCode OK   
        /// if Subject Not Found in DB return HttpStatusCode Not Found   
        /// if id not equal subjectViewModel ID return  HttpStatusCode Bad Request 
        /// if subjectViewModel Not Valid return   HttpStatusCode Bad Request 
        /// </returns>
        [HttpPut("{id}")]
       [Authorize(Roles.Admin)]
        public async Task<IActionResult> Put(int id, [FromForm] AddingSubjectViewModel subjectViewModel)
        {
            if (id != subjectViewModel.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للماده المراد  التعديل عليها  ");
            if (ModelState.IsValid)
            {
                var Status = await _subjects.Update(subjectViewModel);
                if (Status.IsSuccess == true)
                {
                    return Ok(Status);
                }
                else {
                    if (Status.Errors.Count() > 0)
                    {
                        return BadRequest(Status);
                    }
                    return NotFound(Status);
                }
                    
            }
            return BadRequest(subjectViewModel);

        }
        [HttpGet("Search/{Level}")]
      
        public async Task<IActionResult> Search(int Level, int index =0 , int size = 10)
        {
            return Ok(await _subjects.Filter(subj => subj.LevelID == Level , index , size));

        }
        [HttpGet("Search")]
       [Authorize]
        public async Task<IActionResult> Search([FromQuery]string SubjectName, int index = 0, int size = 10)
        {
            return Ok(await _subjects.Filter(subj => subj.Name.Contains(SubjectName) , index , size));

        }

        [HttpDelete("{id}")]
      [Authorize(Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {

            var IsDeleted = await _subjects.Delete(id);

            return IsDeleted is true ?   Ok("تم الحذف  بنجاح ") : BadRequest();
        }
  
    }
}
