using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Reviews.Helper;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.ViewModels.Reviews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private IReviewAsync _Reviews { get; set; }
        public ReviewsController(IReviewAsync review)
        {
            _Reviews = review;
        }
        [HttpGet("Admin/Confirm")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Get(int index =0, int size=20)
        {
            return Ok(await _Reviews.GetAll(index,size));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetReviews(int index=0, int size=20)
        {
            return Ok(await _Reviews.GetReviewsConfirmed(index , size));
        }
         [HttpGet("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Get(int id)
        {
          var Review = await _Reviews.GetAsync(id);
            return Review is not null ? Ok(Review) : NotFound();
        }



        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ReviewViewModel review)
        {
            if (ModelState.IsValid) {
                var reviewadded = await _Reviews.Add(review);
                return reviewadded is null ? BadRequest(review) : Ok("تم الاضافه بنجاح");
            }
            return BadRequest(ModelState);
        }


        [HttpPut("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Put(int id, [FromBody] ReviewUpdatedViewModel review)
        {

            if (id != review.ID) return BadRequest("الرقم التعريفي الذي ادخلته لا يساوي نفس الرقم التعريفي للماده المراد  التعديل عليها  ");
            if (ModelState.IsValid)
            {
                var Status = await _Reviews.UpdateAppearance(review);
                return Status == true ? Ok("تم التعديل بنجاح ") : NotFound();
            }
            return BadRequest(ModelState);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var IsDeleted = await _Reviews.Delete(id);
            return IsDeleted is true ?  Ok("تم الحذف بنجاح") : BadRequest();
        }


    }
}
