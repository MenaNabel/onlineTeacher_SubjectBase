using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Levels.Helper;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class LevelsController : ControllerBase
    {
        private readonly ILevelAsync _levels;
        public LevelsController(ILevelAsync levels)
        {
            _levels = levels;
        }
      
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _levels.GetAll());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
           var Level = await _levels.GetAsync(id);
            return Level is not null ?  Ok(Level) : NotFound();
        }

        [HttpPost]
        //[Authorize(Roles.Admin)]
        public async Task<IActionResult> Post([FromBody] LevelViewModel levelViewModel)
        {
            if (ModelState.IsValid) {
                LevelViewModel leveladded = null;
                try {
                     leveladded = await _levels.Add(levelViewModel);
                }
                catch  {
                    BadRequest("لا ثمكن اضافه اكثر من ثلاث مستويات ");
                }
                return leveladded is null ? BadRequest(levelViewModel) : 
                    Ok("تم الاضافه بنجاح");

            }
            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Put(int id, [FromBody] LevelViewModel levelViewModel)
        {
            if (id != levelViewModel.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للمستوي المراد  التعديل عليها  ");
            if (ModelState.IsValid)
            {

                var leveladded = await _levels.update(levelViewModel);
                return leveladded is false ? NotFound(levelViewModel) :
                    Ok("تم التعديل  بنجاح");

            }
            return BadRequest(ModelState);
        }

   
    }
}
