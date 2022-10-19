using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineTeacher.DataAccess;
using OnlineTeacher.Shared.Interfaces;
using OnlineTeacher.Shared.Services.Emails;
using OnlineTeacher.Shared.Static;
using OnlineTeacher.Shared.ViewModel;
using OnlineTeacher.ViewModels.Identity;
using OnlineTeacher.ViewModels.Identity.Login;
using OnlineTeacher.ViewModels.Identity.Regiesteration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace OnlineTeacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private IUserServices _user;
        private readonly SignInManager<ApplicationUser> _signInManager ;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public AuthController(IUserServices user, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _user = user;
            _signInManager = signInManager;
            _userManager = userManager;
        }
  
        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Regiter([FromBody] RegiesterViewModel model)
        {
            if (ModelState.IsValid) {

               var Result = await _user.RegiesterUserAsync(model);
                if (Result.IsSuccess)
                {
                    return Ok(Result);
                }
                else return BadRequest(Result);
            }
            return BadRequest(ModelState);
        } 
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid) {

               var Result = await _user.LoginUserAsync(model);
                if (Result.IsSuccess)
                {
                   
                    //var subject = ""New login"";
                    //var body = ""<h1> Hey!, new login to your account noticed</h1><p> New login to your account at "" + DateTime.Now + "" </p> "";
                    //await EmailSender.SendEmailAsync(model.Email, subject, body);
                    return Ok(Result);
                }
                else return BadRequest(Result);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("GoogleLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginViewModel model)
        {
            var signInResult = await _signInManager.ExternalLoginSignInAsync("Google", model.GoogleId, isPersistent: false, bypassTwoFactor: false);

            if (!signInResult.Succeeded)
            {
                var email = model.Email;
                if(email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);
                    if(user == null)
                    {
                        var succed = await _user.GoogleRegiesterUserAsync(model);
                        if (succed.IsSuccess)
                        {
                            var loginResult = await _user.GoogleLoginUserAsync(model);
                            if (loginResult.IsSuccess)
                            {
                                return (Ok(loginResult));
                            }
                            else
                            {
                                return BadRequest(loginResult);
                            }
                        }
                        else
                        {
                            return BadRequest(succed);
                        }
                    }

                    UserLoginInfo info = new UserLoginInfo("Google", model.GoogleId,"Google");
                    await _userManager.AddLoginAsync(user, info);
                    var result = await _user.GoogleLoginUserAsync(model);
                    if (result.IsSuccess)
                    {
                        return (Ok(result));
                    } else
                    {
                        return BadRequest(result);
                    }
                } else
                {
                    return BadRequest();
                }
            } else
            {
                var result = await _user.GoogleLoginUserAsync(model);
                if (result.IsSuccess)
                {
                    return (Ok(result));
                }
                else
                {
                    return BadRequest(result);
                }

            }
        }


        [HttpGet("confirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> confirmEmail(string userID, string token) {

            if (string.IsNullOrWhiteSpace(userID) || string.IsNullOrWhiteSpace(token))
                return BadRequest();

          var result =  await _user.ConfirmEmailAsync(userID, token);

            if (result.IsSuccess)
                return Ok("email confirm successfuly");


            return BadRequest(result.Errors);
                    
        }
        [HttpDelete]
        
        public async Task<IActionResult> Delete(string email)
        {
            var res = await _user.DeleteuserByEmail(email);
            if (res.IsSuccess)
                return Ok(res);
            return BadRequest(res);
        }
        [HttpPost("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordViewModel model)
        {
            if (string.IsNullOrEmpty(model.Email))
                return NotFound();

            var result = await _user.ForgetPasswordAsync(model.Email);

            if (result.IsSuccess)
                return Ok(result); // 200

            return BadRequest(result); // 400
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _user.ResetPasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            model.ID = _user.GetCurrentUserID();
            if (ModelState.IsValid)
            {
                var result = await _user.changePasswordAsync(model);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

            return BadRequest("Some properties are not valid");
        }
        //[HttpPost("ChangeIp")]
        //[Authorize(Roles = Roles.Admin)]
        //public async Task<IActionResult> ChangeIp([FromBody] ChangeIpViewModel model)
        //{
            
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _user.ChangeIP(model);

        //        if (result.IsSuccess)
        //            return Ok(result);

        //        return BadRequest(result);
        //    }

        //    return BadRequest("Some properties are not valid");
        //}
        

    }
}
