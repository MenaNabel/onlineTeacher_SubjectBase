using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.Services.Teachers.Helper;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingAsync _setting;
        public SettingController(ISettingAsync setting)
        {
            _setting = setting;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            var setting = await _setting.Get();
            return setting is not null ? Ok(setting) : NotFound();
        }

        [AllowAnonymous]
        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            await _setting.delete();
            return Ok();
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Post([FromForm] AddedSettingViewModel settingViewModel)
        {
            if (ModelState.IsValid)
            {
                if (settingViewModel.FormFile != null)
                {
                    string[] _validExtensions = { "jpg", "bmp", "jpeg" };
                    if (!(settingViewModel.FormFile.Length > 0
                        && _validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 3, 3))
                        || _validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 4, 4).ToLower())))
                    {
                        return BadRequest(new UserMangerResonse("تأكد من رفع صرة بشكل صحيح", false));
                    }
                }


                var AddedSetting = await _setting.Add(settingViewModel);
                return AddedSetting is not null ? Ok("تم الاضافه بنجاح") : BadRequest();

            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles.Admin)]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromForm] AddedSettingViewModel settingViewModel)
        {

            if (ModelState.IsValid)
            {
                if (settingViewModel.FormFile != null)
                {
                    string[] _validExtensions = { "jpg", "png", "jpeg" };
                    if (!(settingViewModel.FormFile.Length > 0
                           && (_validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 3, 3).ToLower())
                           || _validExtensions.Contains(settingViewModel.FormFile.FileName.Substring(settingViewModel.FormFile.FileName.Length - 4, 4).ToLower()))))
                    {
                        return BadRequest(new UserMangerResonse("تأكد من رفع صورة بشكل صحيح", false));
                    }
                }

                bool IsUpdated = await _setting.Update(settingViewModel);
                return IsUpdated is true ? Ok("تم التعديل بنجاح") : NotFound();

            }
            return BadRequest(ModelState);
        }


    }
}
